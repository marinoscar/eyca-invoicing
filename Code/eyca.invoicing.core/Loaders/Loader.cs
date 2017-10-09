using Luval.Orm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Loaders
{
    public class Loader
    {

        protected virtual void OnDoLoad(IEnumerable<object> items)
        {
            OnDoLoad(items, new SqlServerLanguageProvider());
        }

        protected virtual void OnDoLoad(IEnumerable<object> items, ISqlLanguageProvider sqlProvider)
        {
            var connStr = ConfigurationManager.ConnectionStrings["eyca"];
            if (connStr == null) throw new InvalidOperationException("Connection string 'eyca' not set to connect to the database");
            OnDoLoad(items, sqlProvider, connStr.ConnectionString);
        }

        protected virtual void OnDoLoad(IEnumerable<object> items, ISqlLanguageProvider sqlProvider, string connectionString)
        {
            var sql = sqlProvider.Upsert(items);
            var db = new Database(connectionString);
            db.ExecuteNonQuery(sql);
        }
    }
}
