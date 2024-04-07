using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Blog:BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string smallDescription { get; set; } = null!;
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ViewCount { get; set; }
        public string Storage { get; set; } = null!;


    }
}
