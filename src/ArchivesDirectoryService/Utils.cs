using Amazon.Lambda.APIGatewayEvents;

namespace ArchivesDirectoryService;

public static class Utils
{
    public static APIGatewayHttpApiV2ProxyResponse Response(int status, string? body = null)
    {
        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = status,
            Body = body,
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

}
