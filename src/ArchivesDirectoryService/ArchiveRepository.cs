using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace ArchivesDirectoryService;

public sealed class ArchiveRepository(IAmazonDynamoDB dynamoDb)
{
    private readonly IAmazonDynamoDB _dynamoDb = dynamoDb;
    private readonly string _tableName = Environment.GetEnvironmentVariable("DYNAMODB_TABLE_NAME")
            ?? throw new Exception("DYNAMODB_TABLE_NAME not set");

    public async Task<List<Archive>> GetArchivesAsync(string userId, string? parent)
    {
        var request = new QueryRequest
        {
            TableName = _tableName,
            KeyConditionExpression = "user_id = :uid",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                [":uid"] = new() { S = userId }
            }
        };

        if (parent is null)
        {
            request.FilterExpression = "attribute_not_exists(parent)";
        }
        else
        {
            request.FilterExpression = "parent = :parent";
            request.ExpressionAttributeValues[":parent"] =
                new AttributeValue { S = parent };
        }

        var response = await _dynamoDb.QueryAsync(request);

        return response.Items
            .Select(ArchiveMapper.FromItem)
            .ToList();
    }

    public async Task<string> CreateArchiveAsync(Archive archive)
    {
        await _dynamoDb.PutItemAsync(new PutItemRequest
        {
            TableName = _tableName,
            Item = ArchiveMapper.ToItem(archive)
        });

        return archive.ArchiveId;
    }

    public async Task<Archive?> GetArchiveAsync(string userId, string archiveId)
    {
        var response = await _dynamoDb.GetItemAsync(new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                ["user_id"] = new() { S = userId },
                ["archive_id"] = new() { S = archiveId }
            }
        });

        if (response.Item.Count == 0)
            return null;

        return ArchiveMapper.FromItem(response.Item);
    }
}
