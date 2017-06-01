using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core
{
    public class DataObject
    {
        public long ClientId { get; set; }
        public string Client { get; set; }
        public long EngagementId { get; set; }
        public string EngagementName { get; set; }
        public DateTime WeekEndingDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string EmployeeName { get; set; }
        public string Rank { get; set; }
        public double HoursCharged { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityName { get; set; }
        public DateTime ProcessedDate { get; set; }
        public string CategoryCode { get; set; }
        public double ExpenseAmount { get; set; }
        public string Description { get; set; }
        public double HoursDollarAmount { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var prop in GetType().GetProperties())
            {
                sb.AppendFormat("{0}\t",prop.GetValue(this));
            }
            return sb.ToString();
        }

    }
}
