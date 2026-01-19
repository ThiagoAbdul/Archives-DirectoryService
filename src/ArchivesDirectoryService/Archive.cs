using Amazon.DynamoDBv2.DataModel;

namespace ArchivesDirectoryService;

[DynamoDBTable("archives")]
public class Archive
{
    [DynamoDBHashKey("user_id")]
    public string UserId { get; set; }

    [DynamoDBRangeKey("archive_id")]
    public string ArchiveId { get; set; }

    [DynamoDBProperty("content_url")]
    public string? ContentUrl { get; set; }

    [DynamoDBProperty("name")]
    public string Name { get; set; }

    [DynamoDBProperty("parent")]
    public string? Parent { get; set; }

    [DynamoDBProperty("type")]
    public ArchiveType Type { get; set; }

    [DynamoDBProperty("favorite")]
    public bool Favorite { get; set; }
}

public enum ArchiveType
{
    Folder,
    Picture,
    Video,
    Zip
}
