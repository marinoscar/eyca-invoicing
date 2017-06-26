using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core
{
    public class InvoiceExplorer
    {


        public event EventHandler<ProgressEventHandler> ProgressUpdate;

        public List<Dictionary<string, object>> Data { get; private set; }
        public DirectoryInfo Folder { get; private set; }

        public InvoiceExplorer(string rootDir)
        {
            Folder = new DirectoryInfo(rootDir);
            Data = new List<Dictionary<string, object>>();
        }


        public void LoadData()
        {
            var files = GetInvoiceFiles();
            var total = files.Count();
            var i = 1;
            Data = new List<Dictionary<string, object>>();
            foreach (var file in files)
            {
                OnProgressUpdate(i, total, string.Format("File: {0}", file.Name));
                Data.Add(GetDataFromInvoice(file));
                i++;
            }
        }

        protected virtual void OnProgressUpdate(double current, double total, string message)
        {
            OnProgressUpdate(new ProgressEventHandler(current, total, message));
        }

        protected virtual void OnProgressUpdate(ProgressEventHandler e)
        {
            if (ProgressUpdate != null)
                ProgressUpdate(this, e);
        }

        public string ToCsvString()
        {
            if (!Data.Any()) return null;
            var sb = new StringBuilder();
            var isFirst = true;
            foreach (Dictionary<string, object> d in Data)
            {
                if (isFirst)
                {
                    sb.AppendLine(string.Join(",", d.Keys));
                    isFirst = false;
                }
                sb.AppendLine(string.Join(",", d.Values));
            }
            return sb.ToString();
        }

        private IEnumerable<FileInfo> GetInvoiceFiles()
        {
            return Folder.GetFiles("*.xls*", SearchOption.AllDirectories);
        }

        private Dictionary<string, object> GetDataFromInvoice(FileInfo excelFile)
        {
            var res = new Dictionary<string, object>();
            ExcelManager.DoWork(excelFile.FullName, 1, (sheet) =>
            {
                GetDataFromInvoice(sheet, res, excelFile);
            });
            return res;
        }

        private void GetDataFromInvoice(_Worksheet sheet, Dictionary<string, object> d, FileInfo f)
        {
            var val1 = Convert.ToString(sheet.Cells[7, "D"].Value2);
            if (val1 == null || val1 != "EYCA-HOLDING")
                return;
            d["FileName"] = f.Name;
            d["FileCreatedOn"] = f.CreationTime;
            d["FileModifiedOn"] = f.LastWriteTime;
            d["EngagementPartner"] = sheet.Cells[37, "J"].Value2;
            d["EngagementNumber"] = sheet.Cells[37, "D"].Value2;
            d["ProjectCode"] = sheet.Cells[30, "C"].Value2;
            d["TotalForHours"] = sheet.Cells[30, "J"].Value2;
            d["TotalForExpenses"] = sheet.Cells[30, "L"].Value2;
            d["Description"] = sheet.Cells[40, "D"].Value2;
        }

    }
}
