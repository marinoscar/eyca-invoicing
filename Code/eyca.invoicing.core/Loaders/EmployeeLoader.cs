﻿using eyca.invoicing.core.Entities;
using Luval.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Loaders
{
    public class EmployeeLoader : Loader<Employee>, ILoader
    {

        public void DoLoad()
        {
            OnDoLoad(Items);
        }
    }
}
