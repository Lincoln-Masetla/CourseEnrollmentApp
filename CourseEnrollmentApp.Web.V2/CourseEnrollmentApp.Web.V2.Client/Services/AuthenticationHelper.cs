using System.Security.Claims;

namespace CourseEnrollmentApp.Web.V2.Client.Services
{
    public static class AuthenticationHelper
    {
        public static int? GetStudentId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}
