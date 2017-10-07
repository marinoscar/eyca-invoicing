using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core
{
    public class ExcelManager
    {
        public static void DoWork(string fileName, int sheetNum, Action<Application, Workbook, _Worksheet, Range> execute)
        {
            var app = new Application();
            var workbook = app.Workbooks.Open(fileName, ReadOnly: true, UpdateLinks:false, Editable:false);
            _Worksheet sheet = workbook.Sheets[sheetNum];
            var range = sheet.UsedRange;
            try
            {
                execute(app, workbook, sheet, range);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to work on the excel file", ex);
            }
            finally
            {
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(sheet);
                workbook.Close(SaveChanges: false);
                Marshal.ReleaseComObject(workbook);
                app.Quit();
                Marshal.ReleaseComObject(app);
            }
        }

        public static void DoWork(string fileName, int sheetNum, Action<_Worksheet> execute)
        {
            DoWork(fileName, sheetNum, (app, wb, sheet, range) =>
            {
                execute(sheet);
            });
        }

        public static void DoWork(string fileName, int sheetNum, Action<Range> execute)
        {
            DoWork(fileName, sheetNum, (app, wb, sheet, range) =>
            {
                execute(range);
            });
        }

        public static void DoWork(string fileName, int sheetNum, Action<Workbook, _Worksheet, Range> execute)
        {
            DoWork(fileName, sheetNum, (app, wb, sheet, range) =>
            {
                execute(wb, sheet, range);
            });
        }

        public static void DoWork(string fileName, int sheetNum, Action<_Worksheet, Range> execute)
        {
            DoWork(fileName, sheetNum, (app, wb, sheet, range) =>
            {
                execute(sheet, range);
            });
        }
    }
}
