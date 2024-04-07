using HelloJob.Entities.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace HelloJob.App.ViewComponents
{
    public class LoginViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(LoginDto loginModel)
        {
            return View(loginModel);
        }
    }
}
