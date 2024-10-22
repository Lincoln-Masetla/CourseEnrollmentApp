using Bunit;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Core.Interfaces.Services;
using CourseEnrollmentApp.Web.Components.Pages;
using CourseEnrollmentApp.Web.Components.Pages.Accounts;
using CourseEnrollmentApp.Web.Models;
using CourseEnrollmentApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CourseEnrollmentApp.Web.Tests.Components.Pages
{
    [TestFixture]
    public class RegisterComponentTests : Bunit.TestContext
    {
        private Mock<IStudentRepository> _studentRepositoryMock;
        private Mock<IStudentService> _studentServiceMock;
        private CustomAuthenticationStateProvider _authStateProvider;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private DefaultHttpContext _httpContext;
        private Mock<IServiceProvider> _serviceProviderMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _studentServiceMock = new Mock<IStudentService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _serviceProviderMock = new Mock<IServiceProvider>();

            _httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProviderMock.Object
            };
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(_httpContext);

            _authStateProvider = new CustomAuthenticationStateProvider(_httpContextAccessorMock.Object);

            Services.AddSingleton(_studentRepositoryMock.Object);
            Services.AddSingleton(_studentServiceMock.Object);
            Services.AddSingleton<AuthenticationStateProvider>(_authStateProvider);
            Services.AddSingleton<NavigationManager, MockNavigationManager>();

            _serviceProviderMock.Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                                .Returns(new Mock<IAuthenticationService>().Object);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Dispose();
        }

        [Test]
        public void RegisterComponent_ShouldRenderRegistrationForm()
        {
            // Act
            var cut = RenderComponent<Register>();

            // Assert
            var exists = cut.Markup.Contains(@"<h3 class=""card-title text-center"">Student Registration</h3>");
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task RegisterComponent_ShouldShowErrorMessage_WhenEmailIsEmpty()
        {
            // Arrange
            var cut = RenderComponent<Register>();
            var studentDto = new StudentDto { Email = "", Password = "password" };
            cut.Instance.student = studentDto;

            // Act
            await cut.InvokeAsync(() => cut.Instance.RegisterUser());

            // Assert
            cut.Render();
            var exists = cut.Markup.Contains("Email is required.");
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task RegisterComponent_ShouldShowErrorMessage_WhenEmailIsAlreadyRegistered()
        {
            // Arrange
            var cut = RenderComponent<Register>();
            var studentDto = new StudentDto { Email = "test@example.com", Password = "password" };
            cut.Instance.student = studentDto;

            _studentRepositoryMock
                .Setup(repo => repo.GetStudentByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new Student());

            // Act
            await cut.InvokeAsync(() => cut.Instance.RegisterUser());

            // Assert
            cut.Render();
            var exists = cut.Markup.Contains("Email is already registered.");
            Assert.IsTrue(exists);
        }

        [Test]
        public async Task RegisterComponent_ShouldNavigateToCourses_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var cut = RenderComponent<Register>();
            var studentDto = new StudentDto { Email = "test@example.com", Password = "password" };
            cut.Instance.student = studentDto;

            var registeredStudent = new Student { Email = "test@example.com", Password = "password", Id = 1 };
            _studentRepositoryMock
                .Setup(repo => repo.GetStudentByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Student?)null);
            _studentServiceMock
                .Setup(service => service.RegisterStudentAsync(It.IsAny<Student>()))
                .ReturnsAsync(registeredStudent);

            // Act
            await cut.InvokeAsync(() => cut.Instance.RegisterUser());

            // Assert
            await _authStateProvider.MarkUserAsAuthenticated(registeredStudent);
            var navManager = Services.GetRequiredService<NavigationManager>() as MockNavigationManager;
            Assert.AreEqual("http://localhost/courses", navManager?.Uri);
        }
    }
}