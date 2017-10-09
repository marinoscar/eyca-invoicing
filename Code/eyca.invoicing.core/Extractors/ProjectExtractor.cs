using eyca.invoicing.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Extractors
{
    public class ProjectExtractor : ExcelBasedExtractor<Project>
    {
        public ProjectExtractor(string fileName):base(fileName)
        {

        }

        protected override ExcelMapping GetMap()
        {
            return new ExcelMapping()
            {
                RowOffset = 2,
                SheetName = "Project",
                ColumnMap = new List<ExcelAttributeMap>()
                {
                    new ExcelAttributeMap() { Name = "Code", ColumnIndex = 1 },
                    new ExcelAttributeMap() { Name = "EngagementCode", ColumnIndex = 2 },
                    new ExcelAttributeMap() { Name = "LocalProjectId", ColumnIndex = 3 },
                    new ExcelAttributeMap() { Name = "LocalActivityCodeName", ColumnIndex = 4 },
                    new ExcelAttributeMap() { Name = "StartDate", ColumnIndex = 5 },
                    new ExcelAttributeMap() { Name = "EndDate", ColumnIndex = 6 },
                }
            };
        }
    }
}
