using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace CourseEnrollmentApp.Web.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
            return Task.FromResult(new AuthenticationState(user));
        }

        public async Task MarkUserAsAuthenticated(ClaimsPrincipal user)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await httpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                user,
                authProperties);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task MarkUserAsLoggedOut()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            await httpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
