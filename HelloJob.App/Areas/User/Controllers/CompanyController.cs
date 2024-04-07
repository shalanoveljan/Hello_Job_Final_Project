using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace HelloJob.App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Owner")]
    public class CompanyController : Controller
    {
        readonly ICompanyService _CompanyService;
  
        public CompanyController(ICompanyService CompanyService)
        {
            _CompanyService = CompanyService;
        }

        public async Task<IActionResult> Index(string userid,int page = 1,int pagesize=6)
        {
             userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userid;
            var res = await _CompanyService.GetAllAsync(userid, false, page, pagesize);
            return View(res.Datas);
        }

        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create(CompanyPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _CompanyService.CreateAsync(dto);


            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                return View();
            }

            return RedirectToAction(nameof(Index), new { userid = dto.AppUserId });
        }

        public async Task<IActionResult> Update(int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userId;
            var res = await _CompanyService.GetAsync(id);
            return View(res.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]

        public async Task<IActionResult> Update(int id, CompanyPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                var res = await _CompanyService.GetAsync(id);
                return View(res.Data);
            }
            var response = await _CompanyService.UpdateAsync(id, dto);

            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                var res = await _CompanyService.GetAsync(id);
                return View(res.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _CompanyService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
