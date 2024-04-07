using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public  record RequestGetDto
    {
        public int Id { get; set; }
        public VacancyGetDto Vacancy { get; set; }
        public DateTime CreatedAt { get; set; }
        public ResumeGetDto Resume { get; set; }
        public List<VacancyGetDto> Vacancys { get; set; } = null!;

    }
}
