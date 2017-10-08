using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Extractors
{
    public class ExcelExtractor : IExtractor<Dictionary<string, string>>
    {
        public ExcelExtractor(string fileName, ExcelMapping map)
        {
            FileName = fileName;
            Map = map;
        }

        public ExcelMapping Map { get; private set; }
        public string FileName { get; private set; }

        public IEnumerable<Dictionary<string, string>> Extract()
        {
            var res = new List<Dictionary<string, string>>();
            ExcelManager.DoWork(FileName, Map.SheetIndex, (wb, ws, r) => {
                
            });
            return res;
        }
    }
}
