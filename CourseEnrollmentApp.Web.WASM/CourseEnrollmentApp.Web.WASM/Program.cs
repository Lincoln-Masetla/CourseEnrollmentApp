using CourseEnrollmentApp.Application;
using CourseEnrollmentApp.Infrastructure;
using CourseEnrollmentApp.Web.WASM.Components;
using CourseEnrollmentApp.Web.WASM.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

builder.Services.AddAuthentication("TdtsCookie")
     .AddCookie("TdtsCookie", options =>
     {
         options.Cookie.Name = "auth_token";
         options.LoginPath = "/login";
         options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
         options.AccessDeniedPath = "/";
     });
    
builder.Services.AddAuthorization();


builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Initialize the database
app.Services.AddInfrastructureAsync().Wait();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CourseEnrollmentApp.Web.WASM.Client._Imports).Assembly);

app.Run();
