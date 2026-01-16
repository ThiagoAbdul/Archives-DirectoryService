using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace ArchivesDirectoryService.EndpointHandlers;

public abstract class EndpointHandler
{
    protected readonly ArchiveRepository repository = ServiceContainer.Archiverepository;
    public abstract Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context, string userId);

    protected static APIGatewayHttpApiV2ProxyResponse Response(int status, string? body = null) => Utils.Response(status, body);

}
