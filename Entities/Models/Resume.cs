using Entities.Common;
using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Resume:BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        public string Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender {get; set; }
        public JobMode Mode { get; set; }
        public MaritalStatus Status { get; set; }
        public Order order { get; set; }
        public double Salary { get; set; }
        public bool IsDriverLicense { get; set; }
        public bool IsPremium { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int EducationId { get; set; }
        public Education Education { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int Experience { get; set; }
        public DateTime Birthday { get; set; }  
        public DateTime EndDate { get; set; }
        public int ViewCount { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Employee_Special_Education> educations { get; set; }
        public List<Employee_Special_Experience> experiences { get; set; }
        public List<WishlistItem> WishListItems { get; set; }

        public List<Request> Requests { get; set; }

        public Resume()
        {
            Requests= new List<Request>();  
        }
    }
}
