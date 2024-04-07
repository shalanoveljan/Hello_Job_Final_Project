using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace HelloJob.App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Owner")]
    public class VacancyController : Controller
    {
        readonly IVacancyService _VacancyService;
        readonly ICategoryService _categoryService;
        readonly ICityService _cityService;
        readonly ICompanyService _companyService;
        public VacancyController(IVacancyService VacancyService, ICategoryService categoryService, ICityService cityService, ICompanyService companyService)
        {
            _VacancyService = VacancyService;
            _categoryService = categoryService;
            _cityService = cityService;
            _companyService = companyService;
        }

        public async Task<IActionResult> Index(string userid,int page = 1,int pagesize=6)
        {
             userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _VacancyService.GetAllAsync(userid, false, page, pagesize);
            return View(res.Datas);
        }

        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> Create()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Companies = await  _companyService.GetAllAsync(userId,false,1,6);
            ViewBag.UserId = userId;
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Cities = await _cityService.GetAllAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]


        public async Task<IActionResult> Create(VacancyPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Cities = await _cityService.GetAllAsync();
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.Companies = await _companyService.GetAllAsync(userId, false, 1, 6);

                return View();
            }
            var response = await _VacancyService.CreateAsync(dto);
            CompanyGetDto company =(await _companyService.GetAsync(dto.CompanyId)).Data;

            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Cities = await _cityService.GetAllAsync();
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.Companies = await _companyService.GetAllAsync(userId, false, 1, 6);

                ModelState.AddModelError("", response.Message);
                return View();
            }

            return RedirectToAction(nameof(Index), new { userid = company.AppUser.Id });
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.Cities = await _cityService.GetAllAsync();

            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;
            ViewBag.Companies = await _companyService.GetAllAsync(userId, false, 1, 6);

            var res = await _VacancyService.GetAsync(id);
            return View(res.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> Update(int id, VacancyPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Cities = await _cityService.GetAllAsync();
                var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.Companies = await _companyService.GetAllAsync(userId, false, 1, 6);

                var res = await _VacancyService.GetAsync(id);
                return View(res.Data);
            }
            var response = await _VacancyService.UpdateAsync(id, dto);

            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Cities = await _cityService.GetAllAsync();

                ModelState.AddModelError("", response.Message);
                var res = await _VacancyService.GetAsync(id);
                return View(res.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _VacancyService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
