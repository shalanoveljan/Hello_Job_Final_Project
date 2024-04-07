using HelloJob.App.ViewModels;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace HelloJob.App.Controllers
{
    public class RequestController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IRequestService _requestService;
        readonly IResumeService _resumeService;
        readonly IVacancyService _vacancyService;
        readonly ICategoryService categoryService;

        public RequestController(ILogger<HomeController> logger, IRequestService requestService, IResumeService resumeService, IVacancyService vacancyService, ICategoryService categoryService)
        {
            _logger = logger;
            _requestService = requestService;
            _resumeService = resumeService;
            _vacancyService = vacancyService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Create(int vacancyId)
        {
            var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = userid;
            ViewBag.vacancyId = vacancyId;
            RequestVM vm = new RequestVM
            {
                Resumes = (await _resumeService.GetAllAsync(userid, false, 1, 6)).Datas.ToList(),
                Vacancy = (await _vacancyService.GetAsync(vacancyId)).Data,
                Categories = (await categoryService.GetAllAsync()).Datas.ToList(),
        };
            return View(vm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(RequestPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.UserId = userid;
                ViewBag.Resumes = await _resumeService.GetAllAsync(userid, false, 1, 6);
                RequestVM vm = new RequestVM
                {
                    Resumes = (await _resumeService.GetAllAsync(userid, false, 1, 6)).Datas.ToList(),
                    Vacancy = (await _vacancyService.GetAsync(dto.VacancyId)).Data,
                    Categories = (await categoryService.GetAllAsync()).Datas.ToList(),
                };
                return View(vm);
            }
            var res = await _requestService.CreateAsync(dto);
            if (!res.Success)
            {
                var userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.UserId = userid;
                ViewBag.Resumes = await _resumeService.GetAllAsync(userid, false, 1, 6);
                RequestVM vm = new RequestVM
                {
                    Resumes = (await _resumeService.GetAllAsync(userid, false, 1, 6)).Datas.ToList(),
                    Categories = (await categoryService.GetAllAsync()).Datas.ToList(),
                    Vacancy = (await _vacancyService.GetAsync(dto.VacancyId)).Data,

                };
                return View(vm);
            }

            TempData["Request"] = "Send Request for this vacancy";

            return RedirectToAction("index","home");
        }
    }
}