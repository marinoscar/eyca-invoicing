using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eyca.invoicing.core
{
    public class ExcelExtractor_old
    {

        private string _dataFile;


        public ExcelExtractor_old(string dataFile)
        {
            _dataFile = dataFile;
            if (!File.Exists(_dataFile)) throw new ArgumentException("The file does not exists");
        }

        public List<DataObject> GetData()
        {
            var result = new List<DataObject>();
            var f = default(DataObject);
            ExcelManager.DoWork(_dataFile, 1, (sheet, range) => {
                for (int i = 5; i <= range.Rows.Count; i++)
                {

                    var res = ParseExcelRow(range, i, f);
                    if (f == null) f = res;
                    result.Add(res);

                }
            });
            return result;
        }


        private DataObject ParseExcelRow(Range r, int i, DataObject first)
        {
            var obj = new DataObject()
            {
                Client = Val(r, i, 1),
                EngagementName = Val(r,i,2),
                WeekEndingDate = ToDate(Val(r, i, 3)),
                TransactionDate = ToDate(Val(r, i, 4)),
                EmployeeName = Val(r, i, 5),
                Rank = Val(r, i, 7),
                HoursCharged = Convert.ToDouble(Val(r, i, 14)),
                ActivityName = Val(r, i, 20),
                ActivityCode = Val(r, i, 21),
                ProcessedDate = ToDate(Val(r, i, 23)),
                CategoryCode = Val(r, i, 24),
                ExpenseAmount = Convert.ToDouble(Val(r, i, 25)),
                Description = Val(r, i, 27),

            };
            if (first == null)
            {
                obj.ClientId = GetId(Val(r, i, 1));
                obj.EngagementId = GetId(Val(r, i, 2));
            }
            else
            {
                obj.ClientId = first.ClientId;
                obj.EngagementId = first.EngagementId;
            }
            return obj;
        }

        private string Val(Range row, int r, int c)
        {
            if (row[r, c] == null) return null;
            return Convert.ToString(row[r, c].Value2);
        }

        private long GetId(string text)
        {
            var res = Regex.Matches(text, @"\([0-9]{8}\)").Cast<Match>().Where(i => i.Success).Select(i => i.Value).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(res)) return 0;
            return Convert.ToInt64(res.Replace("(", "").Replace(")", ""));
        }

        private DateTime ToDate(string text)
        {
            var num = Convert.ToInt64(text);
            return new DateTime(1900, 1, 1).AddDays(num - 2);
        }


    }
}
