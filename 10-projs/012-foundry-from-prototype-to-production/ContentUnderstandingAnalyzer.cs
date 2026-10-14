using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Azure.Core;

namespace FiberFieldOpsAgent;

internal sealed class ContentUnderstandingAnalyzer(
    HttpClient httpClient,
    TokenCredential credential,
    Uri endpoint,
    string analyzerId,
    string apiVersion,
    string? apiKey)
{
    private static readonly TimeSpan AnalysisTimeout = TimeSpan.FromMinutes(3);

    public async Task<string> AnalyzeAsync(string evidenceUrl, CancellationToken cancellationToken = default)
    {
        if (!Uri.TryCreate(evidenceUrl, UriKind.Absolute, out var evidenceUri)
            || evidenceUri.Scheme != Uri.UriSchemeHttps
            || !string.IsNullOrEmpty(evidenceUri.UserInfo))
        {
            throw new ArgumentException("Evidence must be an absolute HTTPS URL without embedded credentials.", nameof(evidenceUrl));
        }

        var analyzeUri = new Uri(
            endpoint,
            $"contentunderstanding/analyzers/{Uri.EscapeDataString(analyzerId)}:analyze?api-version={Uri.EscapeDataString(apiVersion)}");
        using var request = new HttpRequestMessage(HttpMethod.Post, analyzeUri)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(new { url = evidenceUri.AbsoluteUri }),
                Encoding.UTF8,
                "application/json")
        };
        await AddAuthorizationAsync(request, cancellationToken);

        using var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var operationLocation = response.Headers.Location
            ?? (response.Headers.TryGetValues("Operation-Location", out var values)
                ? new Uri(values.Single(), UriKind.Absolute)
                : null)
            ?? throw new InvalidOperationException("Content Understanding did not return an operation location.");
        ValidateOperationUri(operationLocation);

        var deadline = DateTimeOffset.UtcNow + AnalysisTimeout;
        while (DateTimeOffset.UtcNow < deadline)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);

            using var pollRequest = new HttpRequestMessage(HttpMethod.Get, operationLocation);
            await AddAuthorizationAsync(pollRequest, cancellationToken);
            using var pollResponse = await httpClient.SendAsync(pollRequest, cancellationToken);
            pollResponse.EnsureSuccessStatusCode();

            var json = await pollResponse.Content.ReadAsStringAsync(cancellationToken);
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            var status = root.TryGetProperty("status", out var statusValue)
                ? statusValue.GetString()
                : null;

            if (string.Equals(status, "Succeeded", StringComparison.OrdinalIgnoreCase))
            {
                return root.TryGetProperty("result", out var result)
                    ? result.GetRawText()
                    : root.GetRawText();
            }

            if (string.Equals(status, "Failed", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Content Understanding analysis failed: {root.GetRawText()}");
            }
        }

        throw new TimeoutException("Content Understanding analysis did not complete within three minutes.");
    }

    private async Task AddAuthorizationAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            request.Headers.Add("Ocp-Apim-Subscription-Key", apiKey);
            return;
        }

        var token = await credential.GetTokenAsync(
            new TokenRequestContext(["https://cognitiveservices.azure.com/.default"]),
            cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
    }

    private void ValidateOperationUri(Uri operationUri)
    {
        if (!operationUri.IsAbsoluteUri
            || operationUri.Scheme != Uri.UriSchemeHttps
            || !string.Equals(operationUri.Host, endpoint.Host, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Content Understanding returned an operation URL outside its configured HTTPS endpoint.");
        }
    }
}
