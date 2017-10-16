using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Extractors
{
    public abstract class ExcelBasedExtractor<TEntity> : IExtractor<TEntity>
    {

        private long _maxId;

        public ExcelBasedExtractor(string fileName)
        {
            FileName = fileName;
        }

        protected virtual string FileName { get; private set; }


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

        public virtual IEnumerable<TEntity> Extract()
        {
            var excelExtractor = new ExcelExtractor(FileName, GetMap());
            excelExtractor.ProgressUpdate += ExcelExtractor_ProgressUpdate;
            var items = excelExtractor.Extract().ToList();
            return Transform(items);

        }

        private void ExcelExtractor_ProgressUpdate(object sender, ProgressEventHandler e)
        {
            OnProgressUpdate(e);
        }

        protected virtual List<TEntity> Transform(IEnumerable<Dictionary<string, string>> items)
        {
            var result = new List<TEntity>();
            foreach (var item in items)
            {
                var element = Activator.CreateInstance<TEntity>();
                foreach (var key in item.Keys)
                {
                    var p = element.GetType().GetProperty(key);
                    OnSettingItemValue(p, element, item[key]);
                }
                ApplyId(element);
                result.Add(element);
            }
            return result;
        }


        protected virtual void OnSettingItemValue(PropertyInfo property, object item, object value)
        {
            if (value == null) return;
            if (property.PropertyType == typeof(DateTime))
                HandleExcelDateValues(property, item, value);
            else
                property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
        }

        protected virtual void ApplyId(object item)
        {
            var property = item.GetType().GetProperties().FirstOrDefault(i => i.Name.ToLowerInvariant() == "id");
            if (property == null) return;
            if(_maxId > 0)
                _maxId++;
            else
            {
                _maxId = EntityRowHelper.GetNextId(item.GetType().Name);
            }
            property.SetValue(item, _maxId);
        }

        private void HandleExcelDateValues(PropertyInfo property, object item, object value)
        {
            property.SetValue(item, ConvertToDate(value));
        }

        private DateTime ConvertToDate(object value)
        {
            var anchorDate = new DateTime(1900, 1, 1);
            if (value == null || string.IsNullOrWhiteSpace(Convert.ToString(value)))
                return anchorDate;
            var numValue = Convert.ToInt64(value);
            return anchorDate.AddDays(numValue);
        }

        protected abstract ExcelMapping GetMap();
    }
}
