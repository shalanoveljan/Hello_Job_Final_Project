using HelloJob.App.ViewModels;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{
    public class BlogController : Controller
    {
        readonly IBlogService _blogService;
        readonly ICategoryService _categoryService;

        public BlogController(IBlogService blogService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int PageSize = 6)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(await _blogService.GetAllAsync(pageNumber, PageSize));
        }
        public async Task<IActionResult> Detail(int id)
        {

            var res = await _blogService.GetAsync(id);

            var blogs = await _blogService.GetAllAsync();

            BlogDetailVM vm = new BlogDetailVM
            {
                blogs = blogs.Datas,
                result = res.Data
            };

            await _blogService.IncreaseCount(id);

            return View(vm);
        }



    }
}
