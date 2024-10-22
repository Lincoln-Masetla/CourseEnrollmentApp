using Bunit;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Web.Components.Pages;
using CourseEnrollmentApp.Web.Components.Pages.Accounts;
using CourseEnrollmentApp.Web.Models;
using CourseEnrollmentApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CourseEnrollmentApp.Web.Tests.Components.Pages
{
    [TestFixture]
    public class LoginComponentTests : Bunit.TestContext
    {
        private Mock<IStudentRepository> _studentRepositoryMock;
        private CustomAuthenticationStateProvider _authStateProvider;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private DefaultHttpContext _httpContext;
        private Mock<IServiceProvider> _serviceProviderMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _serviceProviderMock = new Mock<IServiceProvider>();

            _httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProviderMock.Object
            };
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(_httpContext);

            _authStateProvider = new CustomAuthenticationStateProvider(_httpContextAccessorMock.Object);

            Services.AddSingleton(_studentRepositoryMock.Object);
            Services.AddSingleton<AuthenticationStateProvider>(_authStateProvider);
            Services.AddSingleton<NavigationManager, MockNavigationManager>();
            Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                                .Returns(new Mock<IAuthenticationService>().Object);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Dispose();
        }

        [Test]
        public void LoginComponent_ShouldRenderLoginForm()
        {
            // Act
            var cut = RenderComponent<Login>();

            // Assert
            var exists = cut.Markup.Contains(@"<h1 class=""text-center"">Log in</h1>");
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task LoginComponent_ShouldShowErrorMessage_WhenEmailIsEmpty()
        {
            // Arrange
            var cut = RenderComponent<Login>();
            var loginDto = new LoginDto { Email = "", Password = "password" };
            cut.Instance.loginDto = loginDto;

            // Act
            await cut.InvokeAsync(() => cut.Instance.LoginUser());

            // Assert
            cut.Render();
            var exists = cut.Markup.Contains("Email is required.");
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task LoginComponent_ShouldShowErrorMessage_WhenUserNotFound()
        {
            // Arrange
            var cut = RenderComponent<Login>();
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            cut.Instance.loginDto = loginDto;

            _studentRepositoryMock
                .Setup(repo => repo.GetStudentByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Student?)null);

            // Act
            await cut.InvokeAsync(() => cut.Instance.LoginUser());

            // Assert
            cut.Render();
            var exists = cut.Markup.Contains("Invalid email or password.");
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task LoginComponent_ShouldNavigateToHome_WhenLoginIsSuccessful()
        {
            // Arrange
            var cut = RenderComponent<Login>();
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            cut.Instance.loginDto = loginDto;

            var student = new Student { Email = "test@example.com", Password = "password" };
            _studentRepositoryMock
                .Setup(repo => repo.GetStudentByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(student);

            // Act
            await cut.InvokeAsync(() => cut.Instance.LoginUser());

            // Assert
            await _authStateProvider.MarkUserAsAuthenticated(student);
            var navManager = Services.GetRequiredService<NavigationManager>() as MockNavigationManager;
            Assert.AreEqual("http://localhost/", navManager?.Uri);
        }
    }
}
