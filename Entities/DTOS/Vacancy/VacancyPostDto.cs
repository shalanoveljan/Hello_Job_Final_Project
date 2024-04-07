using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record VacancyPostDto
    {
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int RequestId { get; set; }
        public int Salary { get; set; }
        public string Position { get; set; }
        public int CityId { get; set; }
        public int Required_Experience { get; set; }
        public DateTime EndDate { get; set; }
        public JobMode Mode { get; set; }
        public bool IsPremium { get; set; }
        public List<string> Conditions { get; set; }

    }
}
