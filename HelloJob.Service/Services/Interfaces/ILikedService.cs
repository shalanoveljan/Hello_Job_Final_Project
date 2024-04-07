using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Interfaces
{
    public interface ILikedService
    {
      public Task<WishlistGetDto> GetWishList();
     public  Task AddToWishlist(int itemId, string itemtype);
    }
}
