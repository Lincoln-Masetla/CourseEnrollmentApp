using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Web.Services;

namespace CourseEnrollmentApp.Web.Tests
{
    [TestFixture]
    public class CustomAuthenticationStateProviderTests
    {
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private DefaultHttpContext _httpContext;
        private CustomAuthenticationStateProvider _authStateProvider;
        private Mock<IAuthenticationService> _authenticationServiceMock;

        [SetUp]
        public void SetUp()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _httpContext = new DefaultHttpContext();
            _httpContext.RequestServices = new ServiceCollection()
                .AddSingleton(_authenticationServiceMock.Object)
                .BuildServiceProvider();

            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(_httpContext);

            _authStateProvider = new CustomAuthenticationStateProvider(_httpContextAccessorMock.Object);
        }

        [Test]
        public async Task GetAuthenticationStateAsync_ShouldReturnAnonymousUser_WhenHttpContextIsNull()
        {
            // Arrange
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns((HttpContext)null);

            // Act
            var authState = await _authStateProvider.GetAuthenticationStateAsync();

            // Assert
            Assert.IsFalse(authState.User.Identity.IsAuthenticated);
        }

        [Test]
        public async Task GetAuthenticationStateAsync_ShouldReturnAuthenticatedUser_WhenHttpContextHasUser()
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Name, "test@example.com") };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var user = new ClaimsPrincipal(identity);
            _httpContext.User = user;

            // Act
            var authState = await _authStateProvider.GetAuthenticationStateAsync();

            // Assert
            Assert.That(authState.User.Identity.IsAuthenticated, Is.True);
            Assert.That(authState.User.Identity.Name, Is.EqualTo("test@example.com"));
        }

        [Test]
        public async Task MarkUserAsAuthenticated_ShouldSignInUser()
        {
            // Arrange
            var student = new Student { Email = "test@example.com" };

            // Act
            await _authStateProvider.MarkUserAsAuthenticated(student);

            // Assert
            _authenticationServiceMock.Verify(
                x => x.SignInAsync(
                    _httpContext,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    It.Is<ClaimsPrincipal>(p => p.Identity.Name == student.Email),
                    It.IsAny<AuthenticationProperties>()),
                Times.Once);
        }

        [Test]
        public async Task MarkUserAsLoggedOut_ShouldSignOutUser()
        {
            // Act
            await _authStateProvider.MarkUserAsLoggedOut();

            // Assert
            _authenticationServiceMock.Verify(
                x => x.SignOutAsync(
                    _httpContext,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    null),
                Times.Once);
        }
    }
}
