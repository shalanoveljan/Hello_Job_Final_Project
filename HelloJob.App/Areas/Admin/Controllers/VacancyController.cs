using HelloJob.Core.Helper.MailHelper;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class VacancyController : Controller
    {
        readonly IVacancyService _VacancyService;
        readonly ICategoryService _categoryService;
        readonly ICityService _cityService;
        readonly ICompanyService _companyService;
        readonly IEmailHelper _emailHelper;
        public VacancyController(IVacancyService VacancyService, ICategoryService categoryService, IEmailHelper emailHelper, ICityService cityService, ICompanyService companyService)
        {
            _VacancyService = VacancyService;
            _categoryService = categoryService;
            _emailHelper = emailHelper;
            _cityService = cityService;
            _companyService = companyService;
        }

        public async Task<IActionResult> Index(string userid=null,int page = 1, int pagesize = 6)
        {
            return View(await _VacancyService.GetAllAsync(userid,true,page, pagesize));
        }

        public async Task<VacancyGetDto> GetVacancyById(int VacancyId)
        {
            var res = await _VacancyService.GetAsync(VacancyId);
            return res.Data ;
        }
        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            var result = await ProcessOrderStatus(id, "Muracietiniz qebul olundu");
            if (result.Success)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return View(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await ProcessOrderStatus(id, "Muracietiniz red edildi");

            if (result.Success)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return View(nameof(Index));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Pending(int id)
        {
            var result = await ProcessOrderStatus(id, "Muracietiniz gozlemededi");

            if (result.Success)
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return View(nameof(Index));
            }
        }
        private async Task<HelloJob.Core.Utilities.Results.Abstract.IResult> ProcessOrderStatus(int id, string emailSubject)
        {
            var Vacancy = await GetVacancyById(id);

            if (Vacancy == null)
            {
                return new ErrorResult("Vacancy tapilmadi");
            }

            var userEmail = Vacancy.Company.AppUser.Email;

            var orderStatus = GetOrderStatusFromEmailSubject(emailSubject);

            if (orderStatus == Order.None)
            {
                var result = await _VacancyService.SetOrderStatus(id, orderStatus);

                if (result.Success)
                {
                    var notificationResult = await _emailHelper.SendNotificationEmailAsync(userEmail, "Pending", "netice gozlenilir");

                    if (notificationResult.Success)
                    {
                        return new SuccessResult("Pending bildirimi gönderildi");
                    }
                    else
                    {
                        return new ErrorResult("Pending bildirimi gönderilmedi");
                    }
                }
                else
                {
                    return new ErrorResult(result.Message);
                }
            }
            else
            {
                var result = await _VacancyService.SetOrderStatus(id, orderStatus);

                if (result.Success)
                {
                    var notificationResult = await _emailHelper.SendNotificationEmailAsync(userEmail, emailSubject, emailSubject);

                    if (notificationResult.Success)
                    {
                        return new SuccessResult(result.Message);
                    }
                    else
                    {
                        return new ErrorResult("Melumatlandirici e-postası gönderilmedi");
                    }
                }
                else
                {
                    return new ErrorResult(result.Message);
                }
            }
        }

        private Order GetOrderStatusFromEmailSubject(string emailSubject)
        {
            if (emailSubject == "Muracietiniz qebul olundu")
            {
                return Order.Accept;
            }
            else if (emailSubject == "Muracietiniz red edildi")
            {
                return Order.Reject;
            }
            else
            {
                return Order.None;
            }
        }


    }
}
