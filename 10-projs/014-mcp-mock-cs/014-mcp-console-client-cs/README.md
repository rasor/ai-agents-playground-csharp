# MCP Console Client

A .NET 10 console client that discovers tools on the neighboring MCP addition server and calls `add` with `4.2` and `17.9`.

## Run

From this directory, start the server in a separate terminal:

```powershell
dotnet run --project ..\014-mcp-add-cs\McpAddServer.csproj --launch-profile McpAddServer
```

Trust the local ASP.NET Core HTTPS development certificate if it is not already trusted:

```powershell
dotnet dev-certs https --trust
```

Then run the client:

```powershell
dotnet run --project .\014-mcp-console-client-cs.csproj
```

The client connects to `https://localhost:60943/mcp`, prints available tool names and descriptions, invokes `add`, displays its response, and prints `done!` on success. Connection and tool-call errors are written to standard error and result in a non-zero exit code.
