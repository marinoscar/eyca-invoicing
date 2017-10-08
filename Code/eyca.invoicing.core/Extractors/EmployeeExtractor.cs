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
        private List<Employee> _items;

        public EmployeeExtractor(string excelFileName)
        {
            _fileName = excelFileName;
            
        }

        public IEnumerable<Employee> Extract()
        {
            _items = new List<Employee>();
            ExcelManager.DoWork(_fileName, 1, DoRange);
            return null;
        }
 
        private void DoRange(Range r)
        {

        }
    }
}
