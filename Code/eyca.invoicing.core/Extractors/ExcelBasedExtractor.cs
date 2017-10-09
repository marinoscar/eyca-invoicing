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

        public ExcelBasedExtractor(string fileName)
        {
            FileName = fileName;
        }

        protected virtual string FileName { get; private set; }

        public virtual IEnumerable<TEntity> Extract()
        {
            var excelExtractor = new ExcelExtractor(FileName, GetMap());
            var items = excelExtractor.Extract().ToList();
            return Transform(items);

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
                result.Add(element);
            }
            return result;
        }

        protected void OnSettingItemValue(PropertyInfo property, object item, object value)
        {
            if (value == null) return;
            if (property.PropertyType == typeof(DateTime))
                HandleExcelDateValues(property, item, value);
            else
                property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
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
