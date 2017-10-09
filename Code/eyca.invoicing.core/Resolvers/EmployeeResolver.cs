using eyca.invoicing.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Resolvers
{
    public class EmployeeResolver : Resolver<Employee>
    {
        public Employee ByCode(string code)
        {
            return Resolve(i => i.Code == code);
        }
    }
}
