using HelloJob.Entities.DTOS;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HelloJob.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    public class SettingController : Controller
    {
        readonly ISettingService _SettingService;

        public SettingController(ISettingService SettingService)
        {
            _SettingService = SettingService;
        }

        public async Task<IActionResult> Index(int page = 1,int pagesize=6)
        {
            return View(await _SettingService.GetAllAsync(page,pagesize));
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SettingPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res =await _SettingService.CreateAsync(dto);
            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                return View();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _SettingService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var res = await _SettingService.GetAsync(id);
            return View(res.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SettingPostDto dto)
        {
            if (!ModelState.IsValid)
            {

                var result = await _SettingService.GetAsync(id);
                return View(result.Data);
            }

          var res=  await _SettingService.UpdateAsync(id, dto);

            if (!res.Success)
            {
                ModelState.AddModelError("", res.Message);
                var result1 = await _SettingService.GetAsync(id);
                return View(result1.Data);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
