using eyca.invoicing.core.Entities;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Extractors
{
    public class EmployeeExtractor : ExcelBasedExtractor<Employee>
    {

        public EmployeeExtractor(string excelFileName):base(excelFileName)
        {
        }

        protected override ExcelMapping GetMap()
        {
            return new ExcelMapping()
            {
                RowOffset = 2,
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
