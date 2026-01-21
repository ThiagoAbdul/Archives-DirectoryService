using Amazon.DynamoDBv2;

namespace ArchivesDirectoryService;

public class ServiceContainer
{
    private static ServiceContainer? _instance;
    private readonly AmazonDynamoDBClient _dynamoDbClient;

    public ArchiveRepository ArchiveRepository { get; init; }

    private ServiceContainer()
    {

        var region = Environment.GetEnvironmentVariable("AWS_REGION")
            ?? "us-east-1";

        var dynamoConfig = new AmazonDynamoDBConfig
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
        };
        _dynamoDbClient = new(dynamoConfig);

        this.ArchiveRepository = new(_dynamoDbClient);
    }

    public static ServiceContainer Instance { get 
    {
        _instance ??= new();
        return _instance;
    } }
}
