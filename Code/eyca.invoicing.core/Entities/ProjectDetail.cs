using Luval.Orm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Entities
{
    public class ProjectDetail
    {
        [Key]
        public int Id { get; set; }
        [Unique]
        public int EmployeeId { get; set; }
        [Unique]
        public int ProjectId { get; set; }
        public string Rank { get; set; }
        public string Role { get; set; }
        public double Utilization { get; set; }
        public double Rate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
