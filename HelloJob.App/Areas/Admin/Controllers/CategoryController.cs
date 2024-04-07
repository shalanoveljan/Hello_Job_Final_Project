using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoryController : Controller
    {
        readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1,int pagesize=6)
        {
            return View(await _categoryService.GetAllAsync(page,pagesize));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync(1,20);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res =await _categoryService.CreateAsync(dto);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _categoryService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();

            var item = await _categoryService.GetAsync(id);
            return View(item.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CategoryPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                var item = await _categoryService.GetAsync(id);
                return View(item.Data);
            }

          var res=  await _categoryService.UpdateAsync(id, dto);

            if (!res.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ModelState.AddModelError("", res.Message);
                var item = await _categoryService.GetAsync(id);
                return View(item.Data);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
