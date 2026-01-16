using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ArchivesDirectoryService.DTOs;
using System.Text.Json;

namespace ArchivesDirectoryService.EndpointHandlers;

public class ListRootArchivesHandler : EndpointHandler
{
    
    public override async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context, string userId)
    {
        string? parentId = null;
        bool hasFolder = input.QueryStringParameters?.TryGetValue("parent", out parentId) ?? false;

        var archives = await repository.GetArchivesAsync(userId, parentId);

        var content = archives.Select(a => new ArchiveResponse(a));

        var options = new JsonSerializerOptions
        {
            TypeInfoResolver = LambdaFunctionJsonSerializerContext.Default
        };

        var body = JsonSerializer.Serialize(content, options)!;


        return Response(200, body);
    }
}
