using CourseEnrollmentApp.Application.Services;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Core.Interfaces.Services;
using CourseEnrollmentApp.Infrastructure.Data;
using CourseEnrollmentApp.Infrastructure.Repositories;
using CourseEnrollmentApp.Web.Components;
using CourseEnrollmentApp.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/";
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<ICourseRegistrationService, CourseRegistrationService>();

builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<ICourseRegistrationRepository, CourseRegistrationRepository>();

builder.Services.AddScoped(_ => InMemoryDatabase.GetOptions());
builder.Services.AddScoped<ApplicationDbContext>();

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Initialize the database
await InitializeDatabase(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.UseStatusCodePagesWithRedirects("/404");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

async Task InitializeDatabase(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Seed the in-memory database
        InMemoryDatabase.SeedData(dbContext);
        await dbContext.SaveChangesAsync();
    }
}
