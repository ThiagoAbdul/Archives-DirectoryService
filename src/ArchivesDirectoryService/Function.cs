using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArchivesDirectoryService;

public class Function
{

    static ArchiveRepository repository = new();

    private static async Task Main()
    {
        var handler = FunctionHandler;
        await LambdaBootstrapBuilder.Create(handler, new SourceGeneratorLambdaJsonSerializer<LambdaFunctionJsonSerializerContext>())
            .Build()
            .RunAsync();
    }

    public static async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context)
    {

        bool authenticated = input.RequestContext.Authorizer.Jwt.Claims.TryGetValue("sub", out var userId);

        if (!authenticated)
        {
            return new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = 401,
            };
        }
        string? parentId = null;
        bool hasFolder = input.QueryStringParameters?.TryGetValue("parent", out parentId) ?? false;

        context.Logger.LogInformation("sub {}. parent {}", userId, parentId);

        var archives = await repository.GetArchivesAsync(userId!, parentId);

        context.Logger.LogInformation("buscou os arquivos");


        var content = archives.Select(a => new ArchiveResponse(a));

        var options = new JsonSerializerOptions
        {
            TypeInfoResolver = LambdaFunctionJsonSerializerContext.Default
        };

        var body = JsonSerializer.Serialize(content, options)!;


        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Body = body
        };
    }
}

public class ArchiveResponse(Archive archive)
{
    public string UserId { get; } = archive.UserId;
    public string ArchiveId { get; } = archive.ArchiveId;
    public string? ContentUrl { get; } = archive.ContentUrl;
    public string Name { get; } = archive.Name;
    public string? Parent { get; } = archive.Parent;
    public ArchiveType Type { get; } = archive.Type;
    public bool Favorite { get; } = archive.Favorite;
}

[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]
[JsonSerializable(typeof(Archive))]
[JsonSerializable(typeof(ArchiveResponse))]
[JsonSerializable(typeof(IEnumerable<ArchiveResponse>))]
public partial class LambdaFunctionJsonSerializerContext : JsonSerializerContext {}