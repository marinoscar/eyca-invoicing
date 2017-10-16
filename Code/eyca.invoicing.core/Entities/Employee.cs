using Luval.Orm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core.Entities
{
    public class Employee
    {
        [Key]
        public long Id { get; set; }
        [Unique]
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Rank { get; set; }
        public string RankCode { get; set; }

    }
}
