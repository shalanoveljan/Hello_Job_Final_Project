using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class TagCourse:BaseEntity
    {
        public Tag Tag{ get; set; }

        public int TagId { get; set; }

        public Course Course { get; set; }

        public int CourseId { get; set; }
    }
}
