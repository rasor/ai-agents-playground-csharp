using System.ComponentModel;
using McpAddServer.Services;
using ModelContextProtocol.Server;

namespace McpAddServer.Tools;

[McpServerToolType]
public sealed class AdditionTool(AdditionService additionService)
{
    [McpServerTool, Description("Adds two numbers and returns their sum.")]
    public double Add(
        [Description("The first number to add.")] double first,
        [Description("The second number to add.")] double second) =>
        additionService.Add(first, second);
}
