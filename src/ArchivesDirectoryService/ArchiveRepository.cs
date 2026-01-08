
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Linq.Expressions;

namespace ArchivesDirectoryService;

public class ArchiveRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public ArchiveRepository()
    {

        _client = new();
        _context = new DynamoDBContextBuilder()
        .WithDynamoDBClient(() => _client)
        .Build();
    }

    public Task<List<Archive>> GetArchivesAsync(string userId, string? parent)
    {
        ScanCondition filter = parent is null ? 
            new("parent", ScanOperator.IsNull) 
            : new("parent", ScanOperator.Equal, parent);
        var config = new QueryConfig
        {
            QueryFilter = [ filter ] 
        };
         return _context.QueryAsync<Archive>(userId, config).GetRemainingAsync();
    }
}
