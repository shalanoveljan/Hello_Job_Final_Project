using Entities.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class About_Vacancy:BaseEntity
    {
        public string Condition { get; set; }

        public int VacancyId { get; set;  }
        public Vacancy Vacancy { get; set; }


    }

}
