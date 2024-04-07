using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; }
        public Resume Resume { get; set; }
        public int ResumeId { get; set; }
    }
}
