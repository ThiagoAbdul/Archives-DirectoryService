using Amazon.Lambda.APIGatewayEvents;

namespace ArchivesDirectoryService.Extensions;

public static class RequestExtensions
{
    public static string? Me(this APIGatewayHttpApiV2ProxyRequest input)
    {
        _ = input.RequestContext.Authorizer.Jwt.Claims.TryGetValue("sub", out var userId);
        return userId;
    }
}
