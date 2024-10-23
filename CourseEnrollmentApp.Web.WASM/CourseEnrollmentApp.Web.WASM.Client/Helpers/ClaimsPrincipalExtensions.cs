using System.Security.Claims;
using CourseEnrollmentApp.Web.WASM.Client.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace CourseEnrollmentApp.Web.WASM.Client.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static async Task<UserInfo> GetUserInfoAsync(this AuthenticationStateProvider authStateProvider)
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var userId = user.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                var email = user.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

                return (new UserInfo { Email = email, UserId = userId });
            }

            return (null);
        }

    }
}
