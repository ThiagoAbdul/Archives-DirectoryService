using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesDirectoryService.DTOs;

public record CreateFolderCommand(string Name, string? Parent)
{
    
    public Archive ToEntity(string userId)
    {
        return new Archive()
        {
            ArchiveId = Guid.NewGuid().ToString(),
            Name = Name,
            Parent = Parent,
            Type = ArchiveType.Folder,
            UserId = userId 
        };
    }

}
