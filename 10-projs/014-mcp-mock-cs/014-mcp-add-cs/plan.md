# prompt
## plan
* create an asp.net app that hosts a MCP server with a service that can add two numbers
## do
* create plan

---
# Plan: ASP.NET Core MCP Addition Server

## Goal

Create a small ASP.NET Core application that hosts an MCP server and exposes one tool for adding two numbers.

## Scope and assumptions

- Target .NET 10, consistent with the neighboring C# projects.
- Use the official .NET MCP SDK's ASP.NET Core integration and HTTP transport.
- Expose the MCP endpoint at `/mcp`; do not add a separate REST API.
- Keep the addition operation local and deterministic. Azure deployment, persistence, authentication, and additional math tools are out of scope.

## Design

- Create a minimal ASP.NET Core web project in this directory.
- Register an addition service with a method that accepts two numeric operands and returns their sum.
- Expose the operation as an MCP tool named `add`, with clear descriptions for both arguments and the result.
- Map the MCP HTTP endpoint and use standard ASP.NET Core configuration and logging.
- Keep the calculation separate from MCP registration so it can be unit tested independently.

## Implementation steps

1. Create the web project targeting `net10.0` and add the MCP ASP.NET Core package.
2. Implement the addition service and its MCP tool wrapper.
3. Register the service and tool, configure HTTP transport, and map `/mcp`.
4. Add unit tests for positive, negative, zero, and fractional operands.
5. Add an MCP integration check that initializes a client, discovers `add`, invokes it, and verifies the returned sum.
6. Document how to run the server and connect an MCP client.

## Acceptance criteria

- The application builds and starts as an ASP.NET Core server.
- An MCP client can connect to `/mcp`, discover the `add` tool, and call it with two numbers.
- The tool returns the correct sum for positive, negative, zero, and fractional values.
- Tests cover the calculation and the MCP tool invocation.