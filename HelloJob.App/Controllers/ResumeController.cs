using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{

    public class ResumeController : Controller
    {
        readonly IResumeService _ResumeService;
        readonly ICategoryService _categoryService;
        readonly IEducationService _educationService;
        readonly ILanguageService _languageService;
        readonly ILikedService _likeService;

        public ResumeController(IResumeService ResumeService, ICategoryService categoryService, IEducationService educationService, ILanguageService languageService, ILikedService likeService)
        {
            _ResumeService = ResumeService;
            _categoryService = categoryService;
            _educationService = educationService;
            _languageService = languageService;
            _likeService = likeService;
        }


        public async Task<IActionResult> Index(List<ResumeGetDto>? Resumes)
        {

            ViewBag.Categories = await _categoryService.GetAllAsync(1,20);
            ViewBag.Educations = await _educationService.GetAllAsync();
            ViewBag.Languages = await _languageService.GetAllAsync();
            if (Resumes.Count != 0)
            {
                return View(Resumes);
            }
            else
            {
                var result = await _ResumeService.GetAllForResumePageInWebSiteAsync();
                return View(result.Data);
            }
        }
        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.Resumes = await _ResumeService.GetAllForResumePageInWebSiteAsync();
            var res = await _ResumeService.GetAsync(id);
            await _ResumeService.IncreaseCount(id);
            return View(res.Data);
        }

        public async Task<IActionResult> SortResumes(int id, ResumeFilterDto dto)
        {
            ViewBag.Dto=dto;
            var res = await _ResumeService.SortResumes(id,dto);
            return PartialView("_ResumePartial", res.Data);
        }
        [HttpPost]
        public async Task<IActionResult> FilterResumes(ResumeFilterDto dto)
        {
            var res = await _ResumeService.FilterResumes(dto);
           
            return PartialView("_ResumePartial", res.Data );
        }

        public async Task<IActionResult> LoadMore(int id,ResumeFilterDto dto,int pagenumber= 4, int pagesize = 3)
        {
            var res = await _ResumeService.LoadMoreResumesAsync(id,dto,pagenumber,pagesize);
            return PartialView("_ResumePartial", res.Data);
        }

        public async Task<IActionResult> Wishlist()
        {
            
                WishlistGetDto wishlist = await _likeService.GetWishList();
                if (wishlist is null)
                {
                    return Json("Wishlist is null");
                }
                return View(wishlist);
        }
        [HttpPost]
        public async Task<IActionResult> AddWishlist(int itemid, string itemtype)
        {
            await _likeService.AddToWishlist(itemid,itemtype);
            return Json(new
            {
                success = true,
            });

        }

    }
}
