namespace ArchivesDirectoryService.DTOs;

public class ArchiveResponse(Archive archive)
{
    public string UserId { get; } = archive.UserId;
    public string ArchiveId { get; } = archive.ArchiveId;
    public string? ContentUrl { get; } = archive.ContentUrl;
    public string Name { get; } = archive.Name;
    public string? Parent { get; } = archive.Parent;
    public ArchiveType Type { get; } = archive.Type;
    public bool Favorite { get; } = archive.Favorite;
}