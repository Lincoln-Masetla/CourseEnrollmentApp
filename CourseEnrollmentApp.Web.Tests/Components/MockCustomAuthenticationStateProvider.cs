using System.Security.Claims;
using CourseEnrollmentApp.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace CourseEnrollmentApp.Web.Tests.Components
{
    public class MockCustomAuthenticationStateProvider : CustomAuthenticationStateProvider
    {
        public MockCustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.Email, "john.doe@example.com") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);
            return Task.FromResult(authState);
        }

        public virtual Task MockMarkUserAsLoggedOut()
        {
            return Task.CompletedTask;
        }

    }

}
