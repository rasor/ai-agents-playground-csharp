using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace McpAddServer.Tests;

public sealed class McpServerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public McpServerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Mcp_client_can_discover_and_call_add_tool()
    {
        using var httpClient = _factory.CreateClient();
        await using var transport = new HttpClientTransport(
            new HttpClientTransportOptions
            {
                Endpoint = new Uri(httpClient.BaseAddress!, "mcp"),
                TransportMode = HttpTransportMode.StreamableHttp
            },
            httpClient);
        await using var client = await McpClient.CreateAsync(transport);

        var tools = await client.ListToolsAsync();
        Assert.Contains(tools, tool => tool.Name == "add");

        var result = await client.CallToolAsync(
            "add",
            new Dictionary<string, object?> { ["first"] = 1.25, ["second"] = 2.5 });

        Assert.NotEqual(true, result.IsError);
        Assert.Equal("3.75", Assert.IsType<TextContentBlock>(Assert.Single(result.Content)).Text);
    }
}
