using HelloJob.Core.Utilities.Results.Concrete;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace HelloJob.App.Controllers
{
    public class AccountController : Controller
    {
        readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        readonly IWebHostEnvironment _webHostEnvironment;



        public AccountController(IAccountService accountService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Register(RegisterDto dto,string role)
        {
            if(!ModelState.IsValid)
            {
                return View(dto);
            }
           var res= await _accountService.SignUp(dto, role);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);

                return View(dto);
            }
            TempData["Register"] = "Please verify your email";
            return RedirectToAction("index", "Home");
        }

        public async Task<IActionResult> VerifyEmail(string token, string email)
        {
            var result =await _accountService.VerifyEmail(token, email);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            TempData["Verify"] = "Succesfully SignUp";
            return RedirectToAction("index", "home");
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

            var result = await _accountService.Login(dto,false);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(dto);
            }
            return RedirectToAction("index", "home");
        }
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result= await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });

            return Json(claims);
            //return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var result = await _accountService.LogOut();
            return RedirectToAction("index", "home");
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
                return View(email);
            }
            var result = await _accountService.ForgetPassword(email);
            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            TempData["resetPassword"] = "reset";
            return RedirectToAction("index", "home");
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

        //[Authorize]
        //public async Task<IActionResult> Update()
        //{
        //    var query = _userManager.Users.Where(x => x.UserName == User.Identity.Name);
        //    UpdateDto? updateDto = await query.Select(x => new UpdateDto { UserName = x.UserName })
        //        .FirstOrDefaultAsync();
        //    return View(updateDto);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]

        //public async Task<IActionResult> Update(UpdateDto dto)
        //{
        //    if(!ModelState.IsValid) {
        //        return View(dto);
        //    }

        //    var res = await _accountService.Update(dto);
        //    if(!res.Success)
        //    {
        //        ModelState.AddModelError("", res.Message);
        //        return View(dto);
        //    }
        //    TempData["update"] = "updated Account";


        //    return RedirectToAction(nameof(Update));

        //}

    
        [HttpGet]
        public async Task<IActionResult> RegisterWithGoogle(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _accountService.RegisterWithGoogle(returnUrl);
            if (result.Success)
            {
                return RedirectToAction("Home", "Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Google ile kayıt olma işlemi başarısız oldu. Lütfen tekrar deneyin.");
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallback(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _accountService.GoogleCallback(returnUrl);
            if (result.Success)
            {
                return RedirectToAction("Home", "Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to register with Google. Please try again.");
                return View();
            }
        }

    }
}
