using Luval.Orm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Unique]
        public string Code { get; set; }
        public int? EmployeeId { get; set; }
        public string EngagementPartnerName { get; set; }
        public string EngagementPartnerEmail { get; set; }
        public string EngagementParnerOfficeAddress { get; set; }
        public long EngagementCode { get; set; }
        public string EngagementManager { get; set; }
        public string EngagementManagerEmail { get; set; }
        public long PaceNumber { get; set; }
        public long LocalProjectId { get; set; }
        public string LocalActivityCodeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
