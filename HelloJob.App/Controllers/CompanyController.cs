using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.Controllers
{

    public class CompanyController : Controller
    {
        readonly ICompanyService _CompanyService;

        public CompanyController(ICompanyService CompanyService)
        {
            _CompanyService = CompanyService;
        }


        public async Task<IActionResult> Index(List<CompanyGetDto>? Companys)
        {
            if (Companys.Count != 0)
            {
                return View(Companys);
            }
            else
            {
                var result = await _CompanyService.GetAllForCompanyPageInWebSiteAsync();
                return View(result.Data);
            }
        }
        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.Companys = await _CompanyService.GetAllForCompanyPageInWebSiteAsync();
            var res = await _CompanyService.GetAsync(id);
            return View(res.Data);
        }

    }
}
