using HelloJob.App.Configurations;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Data.ServiceRegistrations;
using HelloJob.Service.DependencyResolver;
using HelloJob.Service.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        //options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        //options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
        options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    });


builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(GlobalMapping));
builder.Services.ServiceLayerServiceRegister();
builder.Services.DataAccessServiceRegister(builder.Configuration);

var app = builder.Build();
app.AddDefaultConfiguration(app.Environment);
app.Run();
