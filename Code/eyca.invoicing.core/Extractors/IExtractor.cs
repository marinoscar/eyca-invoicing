using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Extractors
{
    public interface IExtractor<TEntity>
    {
        IEnumerable<TEntity> Extract();
        event EventHandler<ProgressEventHandler> ProgressUpdate;
    }
}
