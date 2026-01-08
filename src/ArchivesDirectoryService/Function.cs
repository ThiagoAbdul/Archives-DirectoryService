using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using ArchivesDirectoryService.DTOs;
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
            return Response(401);
        }

        switch(input.RequestContext.Http.Method)
        {
            case "GET":
                return await GetArchivesAsync(input, userId!, context);
            case "POST":
                return await CreateFolderAsync(input, userId, context);

            default:
                return Response(405);
        }
    }

    private static async Task<APIGatewayHttpApiV2ProxyResponse> GetArchivesAsync(APIGatewayHttpApiV2ProxyRequest input, string userId, ILambdaContext context)
    {
        string? parentId = null;
        bool hasFolder = input.QueryStringParameters?.TryGetValue("parent", out parentId) ?? false;

        context.Logger.LogInformation("antes de buscar no DynamoDB");

        var archives = await repository.GetArchivesAsync(userId!, parentId);

        context.Logger.LogInformation("após buscar no DynamoDB");

        var content = archives.Select(a => new ArchiveResponse(a));

        var options = new JsonSerializerOptions
        {
            TypeInfoResolver = LambdaFunctionJsonSerializerContext.Default
        };

        var body = JsonSerializer.Serialize(content, options)!;

        context.Logger.LogInformation("Antes do response");


        return Response(200, body);

    }

    private static async Task<APIGatewayHttpApiV2ProxyResponse> CreateFolderAsync(APIGatewayHttpApiV2ProxyRequest input, string userId, ILambdaContext context)
    {
        var command = JsonSerializer.Deserialize(input.Body, LambdaFunctionJsonSerializerContext.Default.CreateFolderCommand);

        if (command is null) return Response(400);

        var folderId = await repository.CreateFolderAsync(command.ToEntity(userId));

        return Response(201, folderId);
    }
    private static APIGatewayHttpApiV2ProxyResponse Response(int status, string? body = null)
    {
        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = status,
            Body = body,
            Headers = { { "Content-Type", "application/json" } }
        };
    }
}


[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]
[JsonSerializable(typeof(Archive))]
[JsonSerializable(typeof(ArchiveResponse))]
[JsonSerializable(typeof(IEnumerable<ArchiveResponse>))]
[JsonSerializable(typeof(CreateFolderCommand))]
[JsonSerializable(typeof(string))]
public partial class LambdaFunctionJsonSerializerContext : JsonSerializerContext {}