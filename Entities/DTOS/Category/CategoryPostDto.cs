using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record CategoryPostDto
    {
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? ParentId { get; set; }
    }
}
