using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// --- 1. Agent Card (Discovery) ---
// 他のエージェントがこのエージェントを「見つける」ためのエンドポイント
app.MapGet("/.well-known/agent-card.json", () => new
{
    schema_version = "1.0",
    name = "DotNet-Weather-Agent",
    description = "C# / .NET 9 で実装された A2A エージェントです。",
    capabilities = new[] { "get_forecast" },
    endpoints = new { a2a_rpc = "/rpc" }
});

// --- 2. A2A RPC Endpoint (JSON-RPC 2.0) ---
app.MapPost("/rpc", async (A2ARpcRequest request) =>
{
    // A2Aプロトコルのメソッド判定
    if (request.Method == "get_forecast")
    {
        var city = request.Params?.GetValueOrDefault("city")?.ToString() ?? "Tokyo";
        
        return Results.Ok(new A2ARpcResponse
        {
            Id = request.Id,
            Result = new { temperature = "22C", forecast = "Sunny", location = city }
        });
    }

    return Results.Json(new { 
        jsonrpc = "2.0", 
        error = new { code = -32601, message = "Method not found" }, 
        id = request.Id 
    }, statusCode: 404);
});

app.Run();

// --- Data Models ---
public record A2ARpcRequest(
    [property: JsonPropertyName("jsonrpc")] string JsonRpc,
    [property: JsonPropertyName("method")] string Method,
    [property: JsonPropertyName("params")] Dictionary<string, object>? Params,
    [property: JsonPropertyName("id")] object Id
);

public record A2ARpcResponse
{
    [JsonPropertyName("jsonrpc")] public string JsonRpc { get; init; } = "2.0";
    [JsonPropertyName("result")] public object? Result { get; init; }
    [JsonPropertyName("id")] public object? Id { get; init; }
}
