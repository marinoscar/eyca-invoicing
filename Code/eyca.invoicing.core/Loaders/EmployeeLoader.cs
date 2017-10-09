using eyca.invoicing.core.Entities;
using Luval.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Loaders
{
    public class EmployeeLoader : Loader
    {
        public List<Employee> Items { get; set; }

        public void DoLoad()
        {
            OnDoLoad(Items);
        }
    }
}
