using eyca.invoicing.core.Entities;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Extractors
{
    public class EmployeeExtractor : IExtractor<Employee>
    {

        private string _fileName;
        private List<Employee> _employees;

        public EmployeeExtractor(string excelFileName)
        {
            _fileName = excelFileName;

        }

        public IEnumerable<Employee> Extract()
        {
            var excelExtractor = new ExcelExtractor(_fileName, GetMap());
            var items = excelExtractor.Extract().ToList();
            return TransformItems(items);
        }

        private List<Employee> TransformItems(List<Dictionary<string, string>> items)
        {
            return items.Select(i => new Employee()
            {
                Code = i["Code"],
                Email = i["Email"],
                Name = i["Name"],
                Rank = i["Rank"],
                RankCode = i["RankCode"]
            })
                .ToList();
        }

        private ExcelMapping GetMap()
        {
            return new ExcelMapping()
            {
                RowOffset = 2,
                SheetName = "Employee",
                ColumnMap = new List<ExcelAttributeMap>() {
                    new ExcelAttributeMap() { Name =  "Code", ColumnIndex = 1 },
                    new ExcelAttributeMap() { Name =  "Name", ColumnIndex = 2 },
                    new ExcelAttributeMap() { Name =  "Email", ColumnIndex = 3 },
                    new ExcelAttributeMap() { Name =  "Rank", ColumnIndex = 4 },
                    new ExcelAttributeMap() { Name =  "RankCode", ColumnIndex = 5 },
                }
            };
        }
    }
}
