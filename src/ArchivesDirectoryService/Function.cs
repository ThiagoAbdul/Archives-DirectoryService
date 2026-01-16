using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using ArchivesDirectoryService.DTOs;
using ArchivesDirectoryService.Extensions;
using System.Text.Json.Serialization;

namespace ArchivesDirectoryService;

public class Function
{

    static readonly Router router = new ();

    private static async Task Main()
    {
        var handler = FunctionHandler;
        await LambdaBootstrapBuilder.Create(handler, new SourceGeneratorLambdaJsonSerializer<LambdaFunctionJsonSerializerContext>())
            .Build()
            .RunAsync();
    }

    public static async Task<APIGatewayHttpApiV2ProxyResponse> FunctionHandler(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context)
    {

        string? userId = input.Me();

        if (userId is null)
        {
            return Utils.Response(401);
        }

        return await router.Resolve(input.RouteKey).Handle(input, context, userId);

    }

}


[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyRequest))]
[JsonSerializable(typeof(APIGatewayHttpApiV2ProxyResponse))]
[JsonSerializable(typeof(Archive))]
[JsonSerializable(typeof(ArchiveResponse))]
[JsonSerializable(typeof(IEnumerable<ArchiveResponse>))]
[JsonSerializable(typeof(CreateFolderCommand))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(CreateFileCommand))]
public partial class LambdaFunctionJsonSerializerContext : JsonSerializerContext {}