using eyca.invoicing.core.Extractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Loaders
{
    public class DataLoader<TEntity> : Loader<TEntity>, ILoader
    {

        public DataLoader(IExtractor<TEntity> extractor)
        {
            Extractor = extractor;
            Extractor.ProgressUpdate += Extractor_ProgressUpdate;
        }

        private void Extractor_ProgressUpdate(object sender, ProgressEventHandler e)
        {
            OnProgressUpdate(e);
        }

        public virtual IExtractor<TEntity> Extractor { get; protected set; }

        public event EventHandler<ProgressEventHandler> ProgressUpdate;

        protected virtual void OnProgressUpdate(double current, double total, string message)
        {
            OnProgressUpdate(new ProgressEventHandler(current, total, message));
        }

        protected virtual void OnProgressUpdate(ProgressEventHandler e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        public void DoLoad()
        {
            var items = Extractor.Extract();
            OnDoLoad(items);
        }
    }
}
