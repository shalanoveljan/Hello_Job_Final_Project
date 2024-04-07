using Entities.Common;
using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Course:BaseEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Agency { get; set; }
        public string Description { get; set; }
        public LessonMode Mode { get; set; }
        public Level Level { get; set; }
        public int maxAge { get; set; }
        public int minAge { get; set; }
        public int Period { get; set; } 
        public int Price { get; set; }
        public bool IsRetirement { get; set; }
        public bool IsSertificate { get; set; }

        public bool IsPremium { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<TagCourse> TagsCourse { get; set; }

        public Course()
        {
            TagsCourse=new List<TagCourse>();
        }

        

    }
}
