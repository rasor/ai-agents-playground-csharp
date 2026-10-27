# prompt
## plan
* create a dotnet console app in .\ that calls a mcp server hosted at https://localhost:60943/ having the code in ..\014-mcp-add-cs\.
  * the app should query for which tools it contains, then print them including description.
  * then the app should use tool add with numbers: 4.2, 17.9
  * then say 'done!'
  * if any error catch them and print message.
## do
* create plan

---

# Plan: .NET MCP Console Client

## Goal

Create a small .NET console application that connects to the neighboring MCP addition server, lists its tools and descriptions, invokes `add` with `4.2` and `17.9`, then prints `done!`.

## Scope and assumptions

- Target .NET 10, matching the MCP server project.
- Use the official .NET MCP SDK client and Streamable HTTP transport.
- Connect to the server's MCP endpoint at `https://localhost:60943/mcp`. The server's launch profile supplies the HTTPS port, and its application maps MCP at `/mcp`.
- The server must be running locally before the client is started. Do not silently disable HTTPS certificate validation; document any local development certificate prerequisite.
- Keep the client as a single console workflow; no additional tools, configuration system, or server changes are needed.

## Design

- Create a console project in this directory and reference the MCP client package version compatible with the server's `ModelContextProtocol.AspNetCore` 2.3.0 dependency.
- Create an HTTP client transport targeting the MCP endpoint, connect an MCP client, and list available tools.
- Print each tool's name and description, then call `add` with numeric arguments `first = 4.2` and `second = 17.9`.
- Print the tool's returned content and then `done!` after successful completion.
- Catch and report errors at the top-level workflow, ensuring failures do not print the success message. Dispose the client and transport using the SDK's supported lifetime pattern.

## Implementation steps

1. Create a .NET 10 console project and add the compatible MCP client package.
2. Implement the async MCP connection, tool discovery, and name/description output.
3. Invoke `add` with the specified arguments, print its result, and print `done!` only on success.
4. Add explicit error reporting and ensure MCP resources are disposed.
5. Document how to start the neighboring server and run the client, including the local HTTPS certificate requirement.
6. Build and run the client against the local server to verify discovery and the expected sum.

## Acceptance criteria

- The console project builds on .NET 10.
- Connecting to `https://localhost:60943/mcp` discovers the server's `add` tool and prints its name and description.
- The client invokes `add` with `4.2` and `17.9` and displays the returned sum (`22.1`).
- `done!` is printed only if discovery and invocation complete successfully.
- Connection, protocol, and tool-call errors are caught and their messages are printed.
