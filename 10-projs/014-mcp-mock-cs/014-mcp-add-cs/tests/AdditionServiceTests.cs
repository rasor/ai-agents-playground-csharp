using McpAddServer.Services;
using Xunit;

namespace McpAddServer.Tests;

public sealed class AdditionServiceTests
{
    private readonly AdditionService _service = new();

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(-2, 3, 1)]
    [InlineData(0, 0, 0)]
    [InlineData(1.25, 2.5, 3.75)]
    public void Add_returns_sum(double first, double second, double expected)
    {
        Assert.Equal(expected, _service.Add(first, second));
    }
}
