using System.Globalization;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

const string endpoint = "https://localhost:60943/mcp";

try
{
    await using var transport = new HttpClientTransport(new HttpClientTransportOptions
    {
        Endpoint = new Uri(endpoint),
        TransportMode = HttpTransportMode.StreamableHttp
    });
    await using var client = await McpClient.CreateAsync(transport);

    var tools = await client.ListToolsAsync();
    Console.WriteLine("Available tools:");
    foreach (var tool in tools)
    {
        Console.WriteLine($"- {tool.Name}: {tool.Description}");
    }

    var result = await client.CallToolAsync(
        "add",
        new Dictionary<string, object?>
        {
            ["first"] = 4.2,
            ["second"] = 17.9
        });

    if (result.IsError == true)
    {
        var errorMessage = string.Join(
            Environment.NewLine,
            result.Content.OfType<TextContentBlock>().Select(content => content.Text));
        throw new InvalidOperationException($"The add tool failed: {errorMessage}");
    }

    foreach (var content in result.Content)
    {
        if (content is TextContentBlock textContent)
        {
            Console.WriteLine(double.TryParse(
                textContent.Text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out var numericResult)
                ? numericResult.ToString("G15", CultureInfo.InvariantCulture)
                : textContent.Text);
        }
    }

    Console.WriteLine("done!");
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Error: {exception.Message}");
    Environment.ExitCode = 1;
}
