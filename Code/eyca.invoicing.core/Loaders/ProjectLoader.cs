using eyca.invoicing.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Loaders
{
    public class ProjectLoader : Loader<Project>, ILoader
    {
        public void DoLoad()
        {
            OnDoLoad(Items);
        }
    }
}
