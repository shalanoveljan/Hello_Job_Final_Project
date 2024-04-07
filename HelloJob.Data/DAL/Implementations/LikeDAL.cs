using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.DAL.Implementations
{
    public class WishlistDAL: EFRepositoryBase<Wishlist>, IWishlistDAL
    {
        public WishlistDAL(HelloJobDbContext context) : base(context)
        {

        }
    }

    public class WishlistItemDAL : EFRepositoryBase<WishlistItem>, IWishlistItemDAL
    {
        public WishlistItemDAL(HelloJobDbContext context) : base(context)
        {

        }
    }


}
