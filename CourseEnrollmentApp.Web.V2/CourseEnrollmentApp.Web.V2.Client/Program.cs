using CourseEnrollmentApp.Application;
using CourseEnrollmentApp.Infrastructure;
using CourseEnrollmentApp.Web.V2.Client.Services;
using CourseEnrollmentApp.Web.V2.Client.Services.Contract;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/";
    });

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IStudentClientService, StudentClientService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();


var app = builder.Build();




