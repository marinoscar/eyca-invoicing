using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
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

        public InvoiceCreator(string file, IEnumerable<DataObject> data, string description)
        {
            _templateFile = file;
            _data = new List<DataObject>(data);
            _description = description;
        }

        public void CreateInvoice(DateTime startingOn)
        {
            ExcelManager.DoWork(_templateFile, 2, (app, wb, sheet, range) =>
            {
                WriteRawData(wb);
                UpdateFrontPage(wb);
                LoadTime(wb);
                wb.SaveAs(string.Format(@"C:\Git\eyca-invoicing\Docs\sample-{0}.xls", Guid.NewGuid()));
            });
        }

        private void UpdateFrontPage(Workbook book)
        {
            var sheet = book.Sheets[1];
            sheet.Cells[30, 10] = 1500d;
            sheet.Cells[30, 12] = 500d;
            sheet.Cells[40, 4] = "Nueva descripcion";
        }


        private void LoadTime(Workbook book)
        {
            LoadRates(book);
            var items = _data.Where(i => i.CategoryCode == "Time").ToList();
            var s = book.Sheets[4];
            for (int i = 0; i < items.Count; i++)
            {
                var item = _data[i];
                item.HoursDollarAmount = item.HoursCharged * GetRate(item.Rank);
                s.Cells[i + 2, 1] = item.WeekEndingDate;
                s.Cells[i + 2, 2] = item.EmployeeName;
                s.Cells[i + 2, 3] = item.Rank;
                s.Cells[i + 2, 4] = item.ActivityName;
                s.Cells[i + 2, 5] = item.HoursCharged;
                s.Cells[i + 2, 6] = item.HoursDollarAmount;
                s.Cells[i + 2, 7] = item.Description;
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
                    Code = Convert.ToString(r[i,1].Value2),
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

        private class Rates { public string Code; public string Rank; public double Rate; }

    }
}
