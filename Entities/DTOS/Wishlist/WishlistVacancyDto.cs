using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record WishlistVacancyDto
    {
        public int Id { get; set; }
        public CompanyGetDto Company { get; set; }
        public CategoryGetDto Category { get; set; }
        public int Required_Experience { get; set; }
        public string Position { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewCount { get; set; }
        public int Salary { get; set; }
        public JobMode Mode { get; set; }
        public bool IsPremium { get; set; }
        public bool IsLiked { get; set; }


    }
}
