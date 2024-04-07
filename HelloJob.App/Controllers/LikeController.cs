using HelloJob.App.ViewModels;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloJob.App.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikedService _likeService;
        private readonly UserManager<AppUser> _usermanager;
        private readonly HelloJobDbContext _context;

        public LikeController(ILikedService likeService, UserManager<AppUser> usermanager, HelloJobDbContext context)
        {
            _likeService = likeService;
            _usermanager = usermanager;
            _context = context;
        }

        public async Task<IActionResult> Wishlist()
        {
            WishlistGetDto dto = await _likeService.GetWishList();

            if (dto is null)
            {
                return Json("Wishlist is null");
            }

            return View(dto);
        }


    }
}
