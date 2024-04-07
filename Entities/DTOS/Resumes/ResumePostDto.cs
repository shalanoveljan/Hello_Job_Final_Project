using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record ResumePostDto
    {
        public string AppUserId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public JobMode Mode { get; set; }
        public MaritalStatus Status { get; set; }
        public double Salary { get; set; }
        public bool IsDriverLicense { get; set; }
        public bool IsPremium { get; set; }
        public int CategoryId { get; set; }
        public int EducationId { get; set; }
        public int LanguageId { get; set; }
        public int CityId { get; set; }
        public int Experience { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> SkillNames { get; set; } = null!;
        public List<string> Univerties { get; set; } = null!;
        public List<string> Degrees { get; set; } = null!;
        public List<DateTime> EducationStartDates { get; set; } = null!;
        public List<DateTime> EducationEndDates { get; set; } = null!;
        public List<DateTime> ExperienceStartDates { get; set; } = null!;
        public List<DateTime> ExperienceEndDates { get; set; } = null!;
        public List<string> Workplaces { get; set; } = null!;
        public List<string> Positions { get; set; } = null!;

    }
}
