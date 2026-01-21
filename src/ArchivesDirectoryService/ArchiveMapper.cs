using Amazon.DynamoDBv2.Model;

namespace ArchivesDirectoryService;

public static class ArchiveMapper
{
    public static Dictionary<string, AttributeValue> ToItem(Archive archive)
        => new()
        {
            ["user_id"] = new AttributeValue { S = archive.UserId },
            ["archive_id"] = new AttributeValue { S = archive.ArchiveId },
            ["name"] = new AttributeValue { S = archive.Name },
            ["favorite"] = new AttributeValue { BOOL = archive.Favorite },
            ["type"] = new AttributeValue { S = archive.Type.ToString() }
        };

    public static Archive FromItem(Dictionary<string, AttributeValue> item)
        => new()
        {
            UserId = item["user_id"].S,
            ArchiveId = item["archive_id"].S,
            Name = item["name"].S,
            Favorite = item["favorite"].BOOL ?? false,
            Type = Enum.Parse<ArchiveType>(item["type"].S)
        };
}
