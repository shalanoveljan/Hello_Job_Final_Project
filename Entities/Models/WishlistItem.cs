using Entities.Common;
using HelloJob.Entities.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class WishlistItem:BaseEntity
    {
        public int? ResumeId { get; set; }
        public Resume Resume { get; set; }
        public int? VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
        public int WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }
        public bool IsLiked { get; set; }
    }
}
