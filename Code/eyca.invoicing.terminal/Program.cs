using eyca.invoicing.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            var excelExtractor = new ExcelExtractor(@"C:\Git\eyca-invoicing\Docs\Data.xls");
            var items = excelExtractor.GetData();
            foreach(var d  in items)
            {
                Console.WriteLine(d);
            }
            var writer = new InvoiceCreator(@"C:\Git\eyca-invoicing\Docs\Template.xls", items, "");
            writer.CreateInvoice(DateTime.Today);
            Console.ReadKey();
        }
    }
}
