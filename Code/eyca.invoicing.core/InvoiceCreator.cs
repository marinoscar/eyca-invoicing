using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core
{
    public class InvoiceCreator
    {
        private string _templateFile;
        private List<DataObject> _data;
        private string _description;
        private List<Rates> _rates;
        private List<DataObject> _expenses;
        private List<DataObject> _time;

        public InvoiceCreator(string file, IEnumerable<DataObject> data, string description)
        {
            _templateFile = file;
            _data = new List<DataObject>(data);
            _description = description;
        }

        public string CreateInvoice(DateTime startingOn)
        {
            _data = _data.Where(i => i.WeekEndingDate > startingOn).ToList();
            var fileName = "";
            ExcelManager.DoWork(_templateFile, 2, (app, wb, sheet, range) =>
            {
                UpdateRates(wb);
                _time = _data.Where(i => i.CategoryCode == "Time").OrderBy(i => i.WeekEndingDate).ToList();
                _expenses = _data.Where(i => i.CategoryCode != "Time").OrderBy(i => i.WeekEndingDate).ToList();
                WriteRawData(wb);
                LoadTime(wb);
                LoadExpenses(wb);
                UpdateFrontPage(wb);
                fileName = GetFileName();
                wb.SaveAs(fileName);
            });
            return fileName;
        }

        private void UpdateRates(Workbook wb)
        {
            LoadRates(wb);
            foreach(var d in _data)
            {
                d.HoursDollarAmount = d.HoursCharged * GetRate(d.Rank);
            }
        }

        private void UpdateFrontPage(Workbook book)
        {
            var sheet = book.Sheets[1];
            var timeTotal = _time.Sum(i => i.HoursDollarAmount);
            var expenseTotal = _expenses.Sum(i => i.ExpenseAmount);
            sheet.Cells[30, 10] = timeTotal;
            sheet.Cells[30, 12] = expenseTotal;
            sheet.Cells[40, 4] = GetDescription();
        }


        private void LoadTime(Workbook book)
        {
            var s = book.Sheets[4];
            for (int i = 0; i < _time.Count; i++)
            {
                var item = _time[i];
                s.Cells[i + 2, 1] = item.WeekEndingDate;
                s.Cells[i + 2, 2] = item.EmployeeName;
                s.Cells[i + 2, 3] = item.Rank;
                s.Cells[i + 2, 4] = item.ActivityName;
                s.Cells[i + 2, 5] = item.HoursCharged;
                s.Cells[i + 2, 6] = item.HoursDollarAmount;
                s.Cells[i + 2, 7] = item.Description;
            }
        }

        private string GetDescription()
        {
            var sb = new StringBuilder();
            var month = _time.OrderBy(i => i.WeekEndingDate).Select(i => i.WeekEndingDate).First().ToString("MMMM");
            var employeeTime = _time.Select(i => i.EmployeeName).Distinct().ToList();
            var employeeExpenses = _expenses.Select(i => i.EmployeeName).Distinct().ToList();
            sb.AppendFormat("Hours for the month of {0} ", month);
            sb.AppendFormat("for ");
            foreach (var p in employeeTime)
            {
                sb.AppendFormat("{0} {1} hours ", p, _time.Where(i => i.EmployeeName == p).Sum(i => i.HoursCharged));
            }
            if (employeeExpenses.Any())
            {
                sb.AppendFormat("and expenses from");
                foreach (var p in employeeExpenses)
                {
                    sb.AppendFormat("{0} ", p);
                }
            }
            return sb.ToString();
        }

        private string GetFileName()
        {
            var item = _time.OrderBy(i => i.WeekEndingDate).First();
            var fileInfo = new FileInfo(_templateFile);
            return string.Format(@"{0}\{1}-{2}.xls", fileInfo.Directory, item.EngagementName, item.WeekEndingDate.ToString("MMMM"));
        }

        private void LoadExpenses(Workbook book)
        {
            var s = book.Sheets[4];
            for (int i = 0; i < _expenses.Count; i++)
            {
                var item = _expenses[i];
                s.Cells[i + 2, 1] = item.WeekEndingDate;
                s.Cells[i + 2, 2] = item.EmployeeName;
                s.Cells[i + 2, 3] = item.Rank;
                s.Cells[i + 2, 4] = item.ActivityName;
                s.Cells[i + 2, 5] = item.ExpenseAmount;
                s.Cells[i + 2, 6] = item.Description;
            }
        }



        private double GetRate(string rank)
        {
            var rankCode = rank.Substring(0, 3);
            var item = _rates.FirstOrDefault(i => i.Code == rankCode);
            return item == null ? 0d : item.Rate;
        }

        private void LoadRates(Workbook book)
        {
            _rates = new List<Rates>();
            _Worksheet s = book.Sheets[5];
            var r = s.UsedRange;
            for (int i = 2; i <= r.Rows.Count; i++)
            {
                _rates.Add(new Rates()
                {
                    Code = Convert.ToString(r[i, 1].Value2),
                    Rate = Convert.ToDouble(r[i, 2].Value2),
                    Rank = Convert.ToString(r[i, 3].Value2),
                });
            }

        }

        private void WriteRawData(Workbook book)
        {
            var sheet = book.Sheets[2];
            for (int i = 0; i < _data.Count; i++)
            {
                var item = _data[i];
                sheet.Cells[i + 2, 1] = item.WeekEndingDate;
                sheet.Cells[i + 2, 2] = item.TransactionDate;
                sheet.Cells[i + 2, 3] = item.EmployeeName;
                sheet.Cells[i + 2, 4] = item.Rank;
                sheet.Cells[i + 2, 5] = item.ActivityName;
                sheet.Cells[i + 2, 6] = item.CategoryCode;
                sheet.Cells[i + 2, 7] = item.ExpenseAmount;
                sheet.Cells[i + 2, 8] = item.HoursCharged;
                sheet.Cells[i + 2, 9] = item.Description;
            }
        }

    }

    public class Rates { public string Code { get; set; } public string Rank { get; set; } public double Rate { get; set; } }
}
