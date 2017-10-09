using Luval.Orm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core
{
    public static class Helper
    {
        public static Database GetDb()
        {
            var connStr = ConfigurationManager.ConnectionStrings["eyca"];
            if (connStr == null) throw new InvalidOperationException("Connection string 'eyca' not set to connect to the database");
            return new Database(connStr.ConnectionString);
        }

        public static DbContext GetContext()
        {
            return new DbContext(GetDb());
        }
    }
}
