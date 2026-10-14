#pragma warning disable OPENAI001

using Azure.AI.AgentServer.Core;
using Azure.AI.Projects;
using Azure.Identity;
using DotNetEnv;
using FiberFieldOpsAgent;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry.Hosting;
using Microsoft.Extensions.AI;

Env.NoClobber().TraversePath().Load();

var projectEndpoint = GetRequiredUri("FOUNDRY_PROJECT_ENDPOINT");
var deployment = GetRequiredSetting("AZURE_AI_MODEL_DEPLOYMENT_NAME");
var toolboxName = GetRequiredSetting("TOOLBOX_NAME");
var contentUnderstandingEndpoint = GetRequiredUri("CONTENT_UNDERSTANDING_ENDPOINT");
var contentUnderstandingAnalyzer = GetRequiredSetting("CONTENT_UNDERSTANDING_ANALYZER_ID");
var contentUnderstandingApiVersion =
    Environment.GetEnvironmentVariable("CONTENT_UNDERSTANDING_API_VERSION") ?? "2025-05-01-preview";

var credential = new DefaultAzureCredential();
using var httpClient = new HttpClient();
var contentUnderstanding = new ContentUnderstandingAnalyzer(
    httpClient,
    credential,
    contentUnderstandingEndpoint,
    contentUnderstandingAnalyzer,
    contentUnderstandingApiVersion,
    Environment.GetEnvironmentVariable("CONTENT_UNDERSTANDING_API_KEY"));

var evidenceTool = AIFunctionFactory.Create(
    contentUnderstanding.AnalyzeAsync,
    name: "analyze_fiber_evidence",
    description: "Analyze a publicly accessible HTTPS photo, scan, or report of fiber field evidence with Azure Content Understanding. Return the extracted observations; do not infer a confirmed root cause from an image alone.");

AIAgent agent = new AIProjectClient(projectEndpoint, credential).AsAIAgent(
    model: deployment,
    instructions: FieldOperationsInstructions.Text,
    name: "fiber-field-operations",
    description: "Assists fiber field representatives with evidence-led fault diagnosis.",
    tools: [evidenceTool]);

var builder = AgentHost.CreateBuilder(args);
builder.Services.AddFoundryResponses(agent);
builder.Services.AddFoundryToolboxes(credential, toolboxName);
builder.RegisterProtocol("responses", endpoints => endpoints.MapFoundryResponses());

var app = builder.Build();
app.Run();

static string GetRequiredSetting(string name) =>
    Environment.GetEnvironmentVariable(name) is { Length: > 0 } value
        ? value
        : throw new InvalidOperationException($"Required environment variable '{name}' is not set.");

static Uri GetRequiredUri(string name)
{
    var value = GetRequiredSetting(name);
    if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps)
    {
        throw new InvalidOperationException($"'{name}' must be an absolute HTTPS URL.");
    }

    return uri;
}
