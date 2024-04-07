using Entities.Common;
using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Company:BaseEntity
    {
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public string Name {get; set; }
        public string Image { get; set; }
        public string WebsiteUrlLink { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
        public Order order { get; set; }

        public List<Vacancy> vacancies { get; set; }

    }
}
