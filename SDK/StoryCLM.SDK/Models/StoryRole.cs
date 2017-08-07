using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoryCLM.SDK.Models
{
    public enum ClientRole
    {
        Administrator = 0, // User (Website or Application)
        Editor = 1, // User (Website or Application)
        User = 2, // User (Website or Application)
        Service = 3 // Service
    }
}
