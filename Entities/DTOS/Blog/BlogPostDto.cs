using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record BlogPostDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string smallDescription { get; set; } = null!;
        public IFormFile? ImageFile { get; set; }
        public int CategoryId { get; set; }
    }
}
