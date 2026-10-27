# MCP Add Server

An ASP.NET Core MCP server exposing an `add` tool over Streamable HTTP at `/mcp`.

## Run the server

From this directory:

```powershell
dotnet run --project .\McpAddServer.csproj --urls http://localhost:5000
```

The MCP endpoint is `http://localhost:5000/mcp`.

## Connect an MCP client

For VS Code, add this server to `.vscode/mcp.json`:

```json
{
  "servers": {
    "mcp-add": {
      "type": "http",
      "url": "http://localhost:5000/mcp"
    }
  }
}
```

The `add` tool accepts numeric `first` and `second` arguments and returns their sum.

## Run tests

From this directory:

```powershell
dotnet test .\tests\McpAddServer.Tests.csproj
```
