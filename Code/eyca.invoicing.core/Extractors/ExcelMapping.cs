using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Extractors
{
    public class ExcelMapping
    {
        public ExcelMapping()
        {
            RowOffset = 1;
            SheetIndex = 1;
            ColumnMap = new List<ExcelAttributeMap>();
        }

        public int RowOffset { get; set; }
        public int SheetIndex { get; set; }
        public string SheetName { get; set; }
        public List<ExcelAttributeMap> ColumnMap { get; set; }
    }
}
