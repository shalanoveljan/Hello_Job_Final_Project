using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record WishlistPostDto
    {
        public int Id { get; set; }
        public bool IsLiked { get; set; }
    }
}
