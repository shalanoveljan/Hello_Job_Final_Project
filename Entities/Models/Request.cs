using Entities.Common;
using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    /// <summary>
    /// Resume ile one to one 
    /// Vacancy ile many to many 
    /// </summary>
        //public int VacancyId { get; set; }
        //public Vacancy Vacancy { get; set; }
        //public Resume Resume { get; set; }
        //public int ResumeId { get; set; }
    public class Request:BaseEntity
    {
        public Resume Resume { get; set; }
        public Vacancy Vacancy { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public int ResumeId { get; set; }
        public int VacancyId { get; set; }
    }
}
