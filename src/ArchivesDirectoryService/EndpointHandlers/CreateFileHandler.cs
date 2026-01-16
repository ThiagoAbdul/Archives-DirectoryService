using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;

namespace ArchivesDirectoryService.EndpointHandlers;

public class CreateFileHandler : EndpointHandler
{
    public override async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context, string userId)
    {
        var command = JsonSerializer.Deserialize(input.Body, LambdaFunctionJsonSerializerContext.Default.CreateFileCommand);

        if (command is null) return Response(400);

        if (command.Parent is not null)
        {
            var parent = await repository.GetArchiveAsync(userId, command.Parent);

            if (parent is null) return Response(404, "Parent is null");

            if (parent.Type != ArchiveType.Folder) return Response(422, "parent must be a folder");
        }

        var folderId = await repository.CreateArchiveAsync(command.ToEntity(userId));

        return Response(201, folderId);
    }
}
