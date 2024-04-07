using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class BlogController : Controller
    {
        readonly IBlogService _blogService;
        readonly ICategoryService _categoryService;
        public BlogController(IBlogService blogService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1,int pagesize=6)
        {

            return View(await _blogService.GetAllAsync(page,pagesize));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View();
            }
            var response = await _blogService.CreateAsync(dto);

            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ModelState.AddModelError("", response.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {

            ViewBag.Categories = await _categoryService.GetAllAsync();
            var res = await _blogService.GetAsync(id);
            return View(res.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                var res = await _blogService.GetAsync(id);
                return View(res.Data);
            }
            var response = await _blogService.UpdateAsync(id, dto);

            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ModelState.AddModelError("", response.Message);
                var res = await _blogService.GetAsync(id);
                return View(res.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            var res = await _blogService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
