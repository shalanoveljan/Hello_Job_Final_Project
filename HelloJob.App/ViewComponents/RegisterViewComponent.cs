using HelloJob.Entities.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.ViewComponents
{
    public class RegisterViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RegisterDto registerModel)
        {
            return View(registerModel);
        }
    }
}
