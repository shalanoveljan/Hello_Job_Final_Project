using HelloJob.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record CompanyPostDto
    {
        public string AppUserId { get; set; }
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string WebsiteUrlLink { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
    }
}
