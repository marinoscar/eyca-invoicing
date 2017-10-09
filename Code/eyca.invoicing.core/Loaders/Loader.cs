using Luval.Orm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Loaders
{
    public class Loader<TEntity>
    {
        public Loader()
        {
            Items = new List<TEntity>();
        }

        public List<TEntity> Items { get; set; }

        protected virtual void OnDoLoad(IEnumerable<TEntity> items)
        {
            OnDoLoad(items, new SqlServerLanguageProvider());
        }

        protected virtual void OnDoLoad(IEnumerable<TEntity> items, ISqlLanguageProvider sqlProvider)
        {
            var sql = sqlProvider.Upsert(items);
            var db = Helper.GetDb();
            db.ExecuteNonQuery(sql);
        }
    }
}
