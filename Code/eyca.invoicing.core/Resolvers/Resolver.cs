using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Resolvers
{
    public class Resolver<TEntity>
    {

        public Resolver()
        {
            Items = new List<TEntity>();
        }

        public List<TEntity> Items { get; set; }
        public bool HasLoad { get; private set; }

        public void LoadItems()
        {
            var context = Helper.GetContext();
            Items = context.Select<TEntity>(null).ToList();
            HasLoad = true;
        }

        public TEntity Resolve(Func<TEntity, bool> expression)
        {
            if (!HasLoad) LoadItems();
            return Items.FirstOrDefault(expression);
        }
    }
}
