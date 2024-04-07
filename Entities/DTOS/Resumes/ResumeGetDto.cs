using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record ResumeGetDto
    {
        public AppUser AppUser { get; set; } = null!;
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public JobMode Mode { get; set; }
        public MaritalStatus Status { get; set; }
        public Order order { get; set; }
        public double Salary { get; set; }
        public bool IsDriverLicense { get; set; }
        public bool IsPremium { get; set; }
        public CategoryGetDto Category { get; set; }
        public EducationGetDto Education { get; set; }
        public Language Language { get; set; }
        public CityGetDto City { get; set; }
        public int Experience { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewCount { get; set; }
        public List<Employee_Special_Education> educations { get; set; }
        public List<Employee_Special_Experience> experiences { get; set; }
        public List<Skill> Skills { get; set; }

    }
}
