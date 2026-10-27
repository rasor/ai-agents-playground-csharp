using McpAddServer.Services;
using McpAddServer.Tools;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AdditionService>();
builder.Services
    .AddMcpServer()
    .WithHttpTransport(options => options.Stateless = true)
    .WithTools<AdditionTool>();

var app = builder.Build();

app.MapMcp("/mcp");

app.Run();

public partial class Program { }
