using HelloJob.App.ViewModels;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogService _blogService;
        readonly IVacancyService _VacancyService;
        readonly ICategoryService _categoryService;
        readonly ICityService _cityService;
        readonly ICompanyService _companyService;
        readonly ILikedService _likeService;

        public HomeController(IVacancyService VacancyService, ICategoryService categoryService, ILogger<HomeController> logger, IBlogService blogService, ICompanyService companyService, ICityService cityService, ILikedService likeService)
        {
            _VacancyService = VacancyService;
            _categoryService = categoryService;
            _companyService = companyService;
            _cityService = cityService;
            _blogService = blogService;
            _logger = logger;
            _likeService = likeService;
        }




        public async Task<IActionResult> Index(string searchText,string userid = null, int page = 1, int pagesize = 6)
        {

            HomeVM homeVM = new HomeVM
            {
                categories = await _categoryService.GetAllAsync(1,20),
                blogs = await _blogService.GetAllAsync(page, pagesize),
                vacancies= await _VacancyService.GetVacancysBySearchTextAsync(searchText, page, pagesize),
                companies=await _companyService.GetAllCheckingOrderAsync(page, pagesize)

        };
            return View(homeVM);
        }
        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.Vacancys = await _VacancyService.GetAllForVacancyPageInWebSiteAsync();
            var res = await _VacancyService.GetAsync(id);
            await _VacancyService.IncreaseCount(id);
            return View(res.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AddWishlist(int itemid, string itemtype)
        {
            await _likeService.AddToWishlist(itemid, itemtype);
            return Json(new
            {
                success = true,
            });

        }

    }
}