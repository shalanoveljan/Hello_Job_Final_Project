using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class EducationController : Controller
    {
        readonly IEducationService _EducationService;
        readonly ICategoryService _categoryService;
        public EducationController(IEducationService EducationService, ICategoryService categoryService)
        {
            _EducationService = EducationService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1,int pagesize=6)
        {

            return View(await _EducationService.GetAllAsync(page,pagesize));
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EducationPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _EducationService.CreateAsync(dto);

            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var res = await _EducationService.GetAsync(id);
            return View(res.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, EducationPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                var res = await _EducationService.GetAsync(id);
                return View(res.Data);
            }
            var response = await _EducationService.UpdateAsync(id, dto);

            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                var res = await _EducationService.GetAsync(id);
                return View(res.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            var res = await _EducationService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
