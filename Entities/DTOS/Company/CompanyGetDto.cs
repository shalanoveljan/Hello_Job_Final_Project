using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record CompanyGetDto
    {
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public string Name { get; set; }

        public Order order { get; set; }
        public string Image { get; set; }
        public string WebsiteUrlLink { get; set; }
        public string Email { get; set; }
        public string About { get; set; }

    }
}
