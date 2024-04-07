using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace HelloJob.App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Employee")]
    public class ResumeController : Controller
    {
        readonly IResumeService _ResumeService;
        readonly ICategoryService _categoryService;
        readonly IEducationService _educationService;
        readonly ILanguageService _languageService;
        readonly ICityService _cityService;

        public ResumeController(IResumeService ResumeService, ICategoryService categoryService, IEducationService educationService, ILanguageService languageService, ICityService cityService)
        {
            _ResumeService = ResumeService;
            _categoryService = categoryService;
            _educationService = educationService;
            _languageService = languageService;
            _cityService = cityService;
       
        }

        public async Task<IActionResult> Index(string userid,int page = 1,int pagesize=6)
        {
             userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _ResumeService.GetAllAsync(userid, false, page, pagesize);
            return View(res.Datas);
        }

        [Authorize(Roles = "Employee")]

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Educations = await _educationService.GetAllAsync();
            ViewBag.Languages = await _languageService.GetAllAsync();
            ViewBag.Cities = await _cityService.GetAllAsync();
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]


        public async Task<IActionResult> Create(ResumePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Educations = await _educationService.GetAllAsync();
                ViewBag.Languages = await _languageService.GetAllAsync();
                ViewBag.Cities = await _cityService.GetAllAsync();
                


                return View();
            }
            var response = await _ResumeService.CreateAsync(dto);
            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Educations = await _educationService.GetAllAsync();
                ViewBag.Languages = await _languageService.GetAllAsync();
                ViewBag.Cities = await _cityService.GetAllAsync();
            


                ModelState.AddModelError("", response.Message);
                return View();
            }

            return RedirectToAction(nameof(Index), new { userid = dto.AppUserId });
        }

        public async Task<IActionResult> Update(int id)
        {
            var a=await _categoryService.GetAllAsync();
            ViewBag.Categories = a;
            ViewBag.Educations = await _educationService.GetAllAsync();
            ViewBag.Languages = await _languageService.GetAllAsync();
            ViewBag.Cities = await _cityService.GetAllAsync();
       

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;
            var res = await _ResumeService.GetAsync(id);
            if (!res.Success)
            {

            }
            return View(res.Data);                          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee")]

        public async Task<IActionResult> Update(int id, ResumePostDto dto)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Educations = await _educationService.GetAllAsync();
                ViewBag.Languages = await _languageService.GetAllAsync();
                ViewBag.Cities = await _cityService.GetAllAsync();
              



                var res = await _ResumeService.GetAsync(id);
                return View(res.Data);
            }
            var response = await _ResumeService.UpdateAsync(id, dto);

            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Educations = await _educationService.GetAllAsync();
                ViewBag.Languages = await _languageService.GetAllAsync();
                ViewBag.Cities = await _cityService.GetAllAsync();


                ModelState.AddModelError("", response.Message);
                var res = await _ResumeService.GetAsync(id);
                return View(res.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _ResumeService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
