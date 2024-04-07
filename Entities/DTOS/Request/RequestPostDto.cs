using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record RequestPostDto
    {
        public int SelectedResumeId { get; set; }
        public int VacancyId { get; set; }
        public string AppUserId { get; set; }
    }
}
