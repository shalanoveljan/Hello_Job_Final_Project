using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Education:BaseEntity
    {
        public string Name { get; set; }
        public IEnumerable<Resume> Resumes { get; set; }

    }
}
