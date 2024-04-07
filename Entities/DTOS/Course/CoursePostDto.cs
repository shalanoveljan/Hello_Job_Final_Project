using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record CoursePostDto
    {
        public IFormFile? ImageFile { get; set; }
        public string Title { get; set; }
        public string Agency { get; set; }
        public string Description { get; set; }
        public LessonMode Mode { get; set; }
        public Level Level { get; set; }
        public int maxAge { get; set; }
        public int minAge { get; set; }
        public int Period { get; set; }
        public int Price { get; set; }
        public bool IsRetirement { get; set; }
        public bool IsSertificate { get; set; }
        public bool IsPremium { get; set; }
        public List<int> TagsIds { get; set; } = null!;
        public int CategoryId { get; set; }


    }
}
