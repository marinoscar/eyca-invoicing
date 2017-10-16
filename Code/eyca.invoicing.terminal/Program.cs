using eyca.invoicing.core;
using eyca.invoicing.core.Entities;
using eyca.invoicing.core.Extractors;
using eyca.invoicing.core.Loaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var conArgs = new ConsoleSwitches(args);
            var sw = Stopwatch.StartNew();
            ImportProject(conArgs);
            ImportEmployee(conArgs);
            ImportProjectData(conArgs);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Completed in {0}", sw.Elapsed);
        }

        private static void ImportProject(ConsoleSwitches args)
        {
            if (string.IsNullOrWhiteSpace(args["/project"])) return;
            EntityRowHelper.ZeroRowProject();
            var projectFile = args["/project"];
            var extractor = new ProjectExtractor(projectFile);
            Console.WriteLine("");
            Console.WriteLine("Importing Project Information");
            Console.WriteLine("");
            DoLoad(extractor);
        }

        private static void ImportEmployee(ConsoleSwitches args)
        {
            if (string.IsNullOrWhiteSpace(args["/employee"])) return;
            EntityRowHelper.ZeroRowEmployee();
            var employeeFile = args["/employee"];
            var extractor = new EmployeeExtractor(employeeFile);
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Importing Employee Roster");
            Console.WriteLine("");
            DoLoad(extractor);
        }

        private static void ImportProjectData(ConsoleSwitches args)
        {
            if (string.IsNullOrWhiteSpace(args["/dataFolder"])) return;
            var dataFolder = new DirectoryInfo(args["/dataFolder"]);
            var files = new List<FileInfo>();
            files.AddRange(dataFolder.GetFiles("*.xls*", SearchOption.AllDirectories));
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Importing Project Data");
            Console.WriteLine("");
            var count = 1;
            foreach(var file in files)
            {
                Console.WriteLine("");
                Console.WriteLine("File {0} of {1}", count, files.Count);
                Console.WriteLine("");
                var extractor = new DataExtractor(file.FullName);
                DoLoad(extractor);
                count++;
            }
        }

        private static void DoLoad<T>(IExtractor<T> extractor)
        {
            var loader = new DataLoader<T>(extractor);
            loader.ProgressUpdate += ProgressUpdate;
            loader.DoLoad();
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
            Console.Write("\r{0}      Progress: {1}", e.Message, e.Percentage.ToString("N2"));
        }
    }
}
