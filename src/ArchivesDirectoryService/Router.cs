using ArchivesDirectoryService.EndpointHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesDirectoryService;

public class Router
{
    public readonly ReadOnlyDictionary<string, EndpointHandler> Routes;

    public Router()
    {
        Routes = new (new Dictionary<string, EndpointHandler> 
        {
            { "GET /", new ListRootArchivesHandler() },
            { "POST /folder", new CreateFolderHandler()  },
            { "POST /file", new CreateFileHandler()  },
        });
    }

    public EndpointHandler Resolve(string path)
    {
        bool found = Routes.TryGetValue(path, out var handler);

        if(!found)
        {
            throw new Exception("Enpoint not found");
        }

        return handler!;
    }
}
