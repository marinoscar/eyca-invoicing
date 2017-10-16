using Microsoft.Office.Interop.Excel;
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

        public ExcelMapping Map { get; private set; }
        public string FileName { get; private set; }

        public IEnumerable<Dictionary<string, string>> Extract()
        {
            var result = new List<Dictionary<string, string>>();
            ExcelManager.DoWork(FileName, Map.SheetIndex, (workbook, sheet, range) => {
                var workingSheet = sheet;
                if (!string.IsNullOrWhiteSpace(Map.SheetName))
                    workingSheet = (_Worksheet)workbook.Sheets[Map.SheetName];
                if (workingSheet == null)
                    return;
                var workingRange = workingSheet.UsedRange;
                var totalIterations = workingRange.Rows.Count - Map.RowOffset;
                var current = 0;
                for (int row = Map.RowOffset; row <= workingRange.Rows.Count; row++)
                {
                    var item = new Dictionary<string, string>();
                    foreach (var col in Map.ColumnMap)
                    {
                        item[col.Name] = ExtractStringFromRow(workingRange, row, col.ColumnIndex);
                    }
                    result.Add(item);
                    OnProgressUpdate(current, totalIterations, string.Format("Row {0} of {1}", current + 1, totalIterations));
                    current++;
                }
            });
            return result;
        }

        private string ExtractStringFromRow(Range range, int r, int c)
        {
            if (range[r, c] == null) return null;
            return Convert.ToString(range[r, c].Value2);
        }
    }
}
