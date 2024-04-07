using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record WishlistItemDto
    {
        public string Image { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public JobMode Mode { get; set; }
        public CategoryGetDto Category { get; set; }
        public int Experience { get; set; }
        public DateTime EndDate { get; set; }
        public int ViewCount { get; set; }
        public int Salary { get; set; }
        public bool IsPremium { get; set; }
        public bool IsLiked { get; set; }


    }
}
