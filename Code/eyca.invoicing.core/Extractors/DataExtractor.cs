using eyca.invoicing.core.Entities;
using eyca.invoicing.core.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Extractors
{
    public class DataExtractor : ExcelBasedExtractor<ProjectData>
    {
        public DataExtractor(string fileName) : base(fileName)
        {
        }

        protected override ExcelMapping GetMap()
        {
            return new ExcelMapping()
            {
                RowOffset = 2,
                SheetName = "Data",
                ColumnMap = new List<ExcelAttributeMap>()
                {
                    new ExcelAttributeMap() { Name = "ClientName", ColumnIndex = 1 },
                    new ExcelAttributeMap() { Name = "EngagementName", ColumnIndex = 2 },
                    new ExcelAttributeMap() { Name = "WeekEndDate", ColumnIndex = 3 },
                    new ExcelAttributeMap() { Name = "TransactionDate", ColumnIndex = 4 },
                    new ExcelAttributeMap() { Name = "EmployeeName", ColumnIndex = 5 },
                    new ExcelAttributeMap() { Name = "EmployeeCode", ColumnIndex = 6 },
                    new ExcelAttributeMap() { Name = "Rank", ColumnIndex = 7 },
                    new ExcelAttributeMap() { Name = "HoursCharged", ColumnIndex = 14 },
                    new ExcelAttributeMap() { Name = "BillRate", ColumnIndex = 15 },
                    new ExcelAttributeMap() { Name = "SER", ColumnIndex = 16 },
                    new ExcelAttributeMap() { Name = "NER", ColumnIndex = 18 },
                    new ExcelAttributeMap() { Name = "ERP", ColumnIndex = 19 },
                    new ExcelAttributeMap() { Name = "CostRate", ColumnIndex = 20 },
                    new ExcelAttributeMap() { Name = "ActivityName", ColumnIndex = 22 },
                    new ExcelAttributeMap() { Name = "ActivityCode", ColumnIndex = 23 },
                    new ExcelAttributeMap() { Name = "RecievedFlag", ColumnIndex = 24 },
                    new ExcelAttributeMap() { Name = "ProcessedDate", ColumnIndex = 25 },
                    new ExcelAttributeMap() { Name = "CategoryCode", ColumnIndex = 26 },
                    new ExcelAttributeMap() { Name = "ExpenseAmount", ColumnIndex = 27 },
                    new ExcelAttributeMap() { Name = "CategoryDescription", ColumnIndex = 28 },
                    new ExcelAttributeMap() { Name = "Description", ColumnIndex = 29 },
                    new ExcelAttributeMap() { Name = "SubCategoryDescription", ColumnIndex = 30 },
                }
            };
        }

        protected override List<ProjectData> Transform(IEnumerable<Dictionary<string, string>> items)
        {
            var projectResolver = new ProjectResolver();
            var employeeResolver = new EmployeeResolver();
            var result =  base.Transform(items);
            foreach (var item in result)
            {
                item.IsExpense = (item.CategoryCode != "Time");
                item.ClientId = GetNumbers(item.ClientName);
                item.EngagementId = GetNumbers(item.EngagementName);
                var employee = employeeResolver.ByCode(item.EmployeeCode);
                if (employee != null)
                    item.EmployeeId = employee.Id;
                var project = projectResolver.ByIdAndActivity(item.EngagementId, item.ActivityName);
                if (project != null)
                    item.ProjectId = project.Id;
            }
            return result;
        }

        private long GetNumbers(string text)
        {
            var match = Regex.Matches(text, "(\\d+)").Cast<Match>().FirstOrDefault();
            if (match == null || !match.Success) return 0;
            return Convert.ToInt64(match.Value);
        }


    }
}
