using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    public class CourseController : Controller
    {
        readonly ICourseService _CourseService;
        readonly ITagService _TagService;
        readonly ICategoryService _categoryService;
        public CourseController(ICourseService CourseService, ITagService TagService, ICategoryService categoryService)
        {
            _CourseService = CourseService;
            _TagService = TagService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1,int pagesize=6)
        {

            return View(await _CourseService.GetAllAsync(page,pagesize));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = await _TagService.GetAllAsync();
            ViewBag.Categories = await _categoryService.GetAllAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoursePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                ViewBag.Tags = await _TagService.GetAllAsync();
                return View();
            }
            var response = await _CourseService.CreateAsync(dto);


            if (!response.Success)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();

                ViewBag.Tags = await _TagService.GetAllAsync();
                ModelState.AddModelError("", response.Message);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {

            ViewBag.Tags = await _TagService.GetAllAsync();
            ViewBag.Categories = await _categoryService.GetAllAsync();
            var res = await _CourseService.GetAsync(id);
            return View(res.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CoursePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = await _TagService.GetAllAsync();
                ViewBag.Categories = await _categoryService.GetAllAsync();

                var res = await _CourseService.GetAsync(id);
                return View(res.Data);
            }
            var response = await _CourseService.UpdateAsync(id, dto);

            if (!response.Success)
            {
                ViewBag.Tags = await _TagService.GetAllAsync();
                ViewBag.Categories = await _categoryService.GetAllAsync();

                ModelState.AddModelError("", response.Message);
                var res = await _CourseService.GetAsync(id);
                return View(res.Data);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            var res = await _CourseService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
