using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record BlogGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string smallDescription { get; set; } = null!;
        public string? Image { get; set; } = null!;
        public int ViewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public CategoryGetDto Category { get; set; }
        public int CategoryId { get; set; }

    }
}
