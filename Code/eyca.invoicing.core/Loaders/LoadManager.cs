using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Loaders
{
    public class LoadManager
    {
        public LoadManager()
        {
            Loaders = new List<ILoader>()
            {
                new EmployeeLoader(),
                new ProjectLoader(),
                new DataLoader()
            };
        }

        public List<ILoader> Loaders { get; private set; }

        public void LoadAll()
        {
            Loaders.ForEach(i => i.DoLoad());
        }
    }
}
