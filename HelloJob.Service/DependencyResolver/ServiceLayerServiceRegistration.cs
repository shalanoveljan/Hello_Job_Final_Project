using HelloJob.Core.Helper.MailHelper;
using HelloJob.Service.Services.Implementations;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloJob.Core.Configuration.Abstract;
using HelloJob.Core.Configuration.Concrete;

namespace HelloJob.Service.DependencyResolver
{
    public static class ServiceLayerServiceRegistration
    {
        public static void ServiceLayerServiceRegister(this IServiceCollection services)
        {
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISettingService,SettingService>();
            services.AddScoped<ILayoutService,LayoutService>();
            services.AddScoped<ICourseService,CourseService>();
            services.AddScoped<ITagService,TagService>();
            services.AddScoped<IEducationService,EducationService>();
            services.AddScoped<ILanguageService,LanguageService>();
            services.AddScoped<ICityService,CityService>();
            services.AddScoped<IAccountService,AccountService>();
            services.AddScoped<IResumeService,ResumeService>();
            services.AddScoped<ILikedService,LikedService>();
            services.AddScoped<IRequestService,RequestService>();
            services.AddScoped<IVacancyService,VacancyService>();
            services.AddScoped<ICompanyService,CompanyService>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                                               .ActionContext;
                return new UrlHelper(actionContext);
            });
            services.AddScoped<IEmailHelper,EmailHelper>();
            services.AddScoped<IEmailConfiguration, EmailConfiguration>();
            services.AddHttpContextAccessor();
        }
    }
}
