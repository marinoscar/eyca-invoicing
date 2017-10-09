using Luval.Orm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Entities
{
    public class ProjectData
    {
        [Key, AutoIncrement]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        [Unique]
        public string ClientName { get; set; }
        public long ClientId { get; set; }
        [Unique]
        public string EngagementName { get; set; }
        public long EngagementId { get; set; }
        public DateTime WeekEndDate { get; set; }
        [Unique]
        public DateTime TransactionDate {get;set;}
        public DateTime ProcessedDate { get; set; }
        public string EmployeeName { get; set; }
        [Unique]
        public string EmployeeCode { get; set; }
        public string Rank {get;set;}
        public double HoursCharged { get; set; }
        public double BillRate { get; set; }
        public double SER { get; set; }
        public double NER { get; set; }
        public double ERP { get; set; }
        public double CostRate { get; set; }
        public string ActivityName { get; set; }
        public string ActivityCode { get; set; }
        public string RecievedFlag { get; set; }
        public string CategoryCode { get; set; }
        public double ExpenseAmount { get; set; }
        public string CategoryDescription { get; set; }
        public string Description { get; set; }
        public string SubCategoryDescription { get; set; }
        [Unique]
        public bool IsExpense { get; set; }
    }
}
