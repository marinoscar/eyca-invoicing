using eyca.invoicing.core;
using eyca.invoicing.core.Extractors;
using eyca.invoicing.core.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            var extractor = new EmployeeExtractor(@"C:\Git\eyca-invoicing\Docs\Employee.xlsx");
            var res = extractor.Extract();
            var loader = new EmployeeLoader() { Items = res.ToList() };
            loader.DoLoad();


            //var arguments = new ConsoleSwitches(args);
            //if (arguments.ContainsSwitch("/createInvoice")) CreateInvoices(arguments);
            //if (arguments.ContainsSwitch("/invoiceReport")) InvoiceReport(arguments);
        }

        private static void InvoiceReport(ConsoleSwitches args)
        {
            var dir = args["-d"];
            var output = args["-o"];
            var inv = new InvoiceExplorer(dir);
            inv.ProgressUpdate += ProgressUpdate;
            inv.LoadData();
            var content = inv.ToCsvString();
            File.WriteAllText(output, content);
        }

        private static void ProgressUpdate(object sender, ProgressEventHandler e)
        {
            Console.WriteLine("{0}      Progress: {1}", e.Message, e.Percentage.ToString("N2"));
        }

        private static void CreateInvoices(ConsoleSwitches args)
        {
            var date = DateTime.Parse(args["-d"]);
            var file = args["-f"];
            var template = args["-t"];
            var excelExtractor = new ExcelExtractor_old(file);
            Console.WriteLine();
            Console.WriteLine("Creating Invoice with data from {0} for template {1} filtering by date {2}", file, template, date);
            var items = excelExtractor.GetData();
            var writer = new InvoiceCreator(template, items, "");
            var newFile = writer.CreateInvoice(date);
            Console.WriteLine();
            Console.WriteLine("File {0} has been created", newFile);
            Console.WriteLine();
        }
    }
}
