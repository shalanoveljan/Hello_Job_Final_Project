using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Employee_Special_Education:BaseEntity
    {
        public string University { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ResumeId { get; set; }
        public Resume Resume { get; set; }
    }
}
