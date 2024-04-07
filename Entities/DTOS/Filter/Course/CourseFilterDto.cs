using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record CourseFilterDto
    {
        public List<int> CategoriesIds { get; set; }
        public int MinTime { get; set; }
        public int MaxTime { get; set; } = 24;

        public List<string> Agencies { get; set; }
        public bool IsFree { get; set; } 
        public bool IsSertificate { get; set; }

        public bool IsRetirement { get; set; }

        public LessonMode Selected_Lesson_Mode { get; set; }

        public Level  Level { get; set; }

        public bool IsSort { get; set; }

    }

   
}
