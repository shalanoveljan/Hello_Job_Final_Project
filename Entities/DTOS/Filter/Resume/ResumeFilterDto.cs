using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record ResumeFilterDto
    {
        public List<int> CategoriesIds { get; set; }
        public List<int> LanguagesIds { get; set; }
        public List<int> EducationsIds { get; set; }
        public int MinSalary { get; set; } = 100;
        public int MaxSalary { get; set; } = 10000;
        public int MinExperience { get; set; }
        public int MaxExperience { get; set; }=6;
        public bool IsDriverLicense { get; set; }
        public Gender Gender { get; set; }
        public JobMode Mode { get; set; }
        public MaritalStatus Status { get; set; }
        public bool IsSort { get; set; }

    }

   
}
