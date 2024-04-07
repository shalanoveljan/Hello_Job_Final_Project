using HelloJob.Data.DAL.Implementations;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.ServiceRegistrations
{
    public static class DataAccessServiceRegistration
    {
        public static void DataAccessServiceRegister(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HelloJobDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });


            services.AddScoped<IBlogDAL, BlogDAL>();
            services.AddScoped<ICategoryDAL, CategoryDAL>();
            services.AddScoped<ISettingDAL, SettingDAL>();
            services.AddScoped<ITagDAL, TagDAL>();
            services.AddScoped<ICourseDAL, CourseDAL>();
            services.AddScoped<ILayoutDAL, LayoutDAL>();
            services.AddScoped<ILanguageDAL, LanguageDAL>();
            services.AddScoped<IEducationDAL, EducationDAL>();
            services.AddScoped<ICityDAL, CityDAL>();
            services.AddScoped<IResumeDAL, ResumeDAL>();
            services.AddScoped<IWishlistDAL, WishlistDAL>();
            services.AddScoped<IWishlistItemDAL, WishlistItemDAL>();
            services.AddScoped<ICompanyDAL, CompanyDAL>();
            services.AddScoped<IVacancyDAL, VacancyDAL>();
            services.AddScoped<IRequestDAL, RequestDAL>();

            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<HelloJobDbContext>()
                .AddDefaultTokenProviders();

        }
    }
}
