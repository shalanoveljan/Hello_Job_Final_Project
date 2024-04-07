using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace HelloJob.App.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "Owner")]
    public class RequestController : Controller
    {
        readonly IRequestService _RequestService;

        public RequestController(IRequestService requestService)
        {
            _RequestService = requestService;
        }
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Index(string userid,int page = 1,int pagesize=6)
        {
             userid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _RequestService.GetAllAsync(userid,page, pagesize);
            if (res == null)
            {
                return NotFound();
            }
            return View(res.Datas);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var res = await _RequestService.RemoveAsync(id);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
