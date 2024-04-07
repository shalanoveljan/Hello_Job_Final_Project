using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record WishlistGetDto
    {
        public List<WishlistItemDto> wishlistItems { get; set; }
        public List<WishlistVacancyDto> wishlistVacancies { get; set; }
        public WishlistGetDto()
        {
            wishlistItems= new List<WishlistItemDto>();
            wishlistVacancies= new List<WishlistVacancyDto>();
        }
    }
}
