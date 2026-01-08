using Amazon.DynamoDBv2.DataModel;

namespace ArchivesDirectoryService;

[DynamoDBTable("archives")]
public class Archive
{
    [DynamoDBHashKey("user_id")]
    public string UserId { get; set; }

    [DynamoDBRangeKey("archive_id")]
    public string ArchiveId { get; set; }

    public string? ContentUrl { get; set; }
    public string Name { get; set; }
    public string? Parent { get; set; }
    public ArchiveType Type { get; set; }
    public bool Favorite { get; set; }
}

public enum ArchiveType
{
    Folder,
    Picture,
    Video
}
