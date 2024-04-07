using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{
    public class CourseController : Controller
    {
        readonly ICourseService _CourseService;
        readonly ICategoryService _categoryService;
        readonly ITagService _tagService;

        public CourseController(ICourseService CourseService, ICategoryService categoryService, ITagService tagService)
        {
            _CourseService = CourseService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(List<CourseGetDto>? courses)
        {

            ViewBag.Categories = await _categoryService.GetAllAsync(1,20);
            if (courses.Count != 0)
            {

                return View(courses);
            }
            else
            {
                var result = await _CourseService.GetAllForCoursePageInWebSiteAsync();
                return View(result.Data);
            }
        }
        public async Task<IActionResult> Detail(int id)
        {
            var res = await _CourseService.GetAsync(id);
            return View(res.Data);
        }

        public async Task<IActionResult> SortCourses(int id, CourseFilterDto dto)
        {
            ViewBag.Dto=dto;
            var res = await _CourseService.SortCourses(id,dto);
            return PartialView("_CoursePartial", res.Data);
        }
        [HttpPost]
        public async Task<IActionResult> FilterCourses(CourseFilterDto dto)
        {
            var res = await _CourseService.FilterCourses(dto);
            return PartialView("_CoursePartial", res.Data);

        }



    }
}
