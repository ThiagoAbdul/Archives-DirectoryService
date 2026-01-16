using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArchivesDirectoryService.DTOs;

public record CreateFileCommand(string ArchiveId, string Name, ArchiveType ArchiveType, string? Parent)
{
    public Archive ToEntity(string userId)
    {
        return new Archive()
        {
            ArchiveId = ArchiveId,
            Name = Name,
            Parent = Parent,
            Type = ArchiveType,
            UserId = userId
        };
    }
}
