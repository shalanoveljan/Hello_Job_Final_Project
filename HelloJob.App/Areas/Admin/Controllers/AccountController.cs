using HelloJob.Core.Utilities.Results.Concrete;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IAccountService accountService, UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]   

        public async Task<IActionResult> CreateAdmin(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var res = await _accountService.SignUp(dto, "Admin");
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                return View(dto);
            }
            TempData["Register"] = "Please verify your email";
            return RedirectToAction("index", "Dashboard");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllAdmins(int page=1, int count=4)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var res = await _accountService.GetAllAdmin(page,count);
            return View(res);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllUsers(int page = 1, int count = 4)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var res = await _accountService.GetAllUsers(page, count);
            return View(res);
        }
        //[Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var result = await _accountService.VerifyEmail(token, email);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            TempData["Verify"] = "Succesfully SignUp";
            return RedirectToAction("index", "dashboard");
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Login(LoginDto dto)
        {

            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _accountService.Login(dto,true);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }
            return RedirectToAction("index", "dashboard",new {area = "Admin"});
        }

        public async Task<IActionResult> LogOut()
        {
            var result = await _accountService.LogOut();
            return RedirectToAction("login", "account");
        }
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _accountService.ForgetPassword(email);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(email);
            }
            TempData["reset"] = "check email";
            return RedirectToAction("index", "dashboard");
        }


        [HttpGet]

        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (!ModelState.IsValid)
            {
                return View(email);
            }

            var result = await _accountService.ResetPasswordGet(email, token);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(email);
            }
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _accountService.ResetPasswordPost(dto);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }
            return RedirectToAction("login", "Account");
        }


        [Authorize]
        [Authorize(Roles = "SuperAdmin,Admin")]

        public async Task<IActionResult> Update()
        {
            var query = _userManager.Users.Where(x => x.UserName == User.Identity.Name);
            UpdateDto? updateDto = await query.Select(x => new UpdateDto { UserName = x.UserName })
                .FirstOrDefaultAsync();
            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "SuperAdmin,Admin")]


        public async Task<IActionResult> Update(UpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var res = await _accountService.Update(dto);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                return View(dto);
            }
            TempData["update"] = "updated Account";


            return RedirectToAction(nameof(Update));

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeActivationStatus(string email, bool activate)
        {
            var result = await _accountService.ChangeUserActivationStatus(email, activate);

            if (result.Success)
            {
                TempData["update"] = "changed Activated status";
                return Redirect(Request.Headers["Referer"].ToString());

            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> ChangeUserRole(string userId, string newRoleId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newRoleId))
            {
                return BadRequest("userid or newRoleID is null");
            }

            var result = await _accountService.ChangeRole(userId, newRoleId);
            if (result)
            {
                return RedirectToAction("Index","Dashboard");
            }
            else
            {
                return BadRequest("dont change role unfortunately");
            }
        }

    }
}
