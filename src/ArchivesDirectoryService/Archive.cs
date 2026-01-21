namespace ArchivesDirectoryService;

public sealed class Archive
{
    public string UserId { get; init; } = default!;
    public string ArchiveId { get; init; } = default!;
    public string? ContentUrl { get; init; }
    public string Name { get; init; } = default!;
    public string? Parent { get; init; }
    public ArchiveType Type { get; init; }
    public bool Favorite { get; init; }
}

public enum ArchiveType
{
    Folder,
    Picture,
    Video,
    Zip
}