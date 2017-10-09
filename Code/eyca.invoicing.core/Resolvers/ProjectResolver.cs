using eyca.invoicing.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Resolvers
{
    public class ProjectResolver : Resolver<Project>
    {
        public Project ByIdAndActivity(long localId, string activity)
        {
            return Resolve(i => i.LocalProjectId == localId && i.LocalActivityCodeName == activity);
        }
    }
}
