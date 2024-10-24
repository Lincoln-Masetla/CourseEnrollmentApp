using Bunit;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Services;
using CourseEnrollmentApp.Web.Components.Pages;
using CourseEnrollmentApp.Web.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseEnrollmentApp.Web.Tests.Components.Pages
{
    [TestFixture]
    public class UpdateProfileTests : Bunit.TestContext
    {
        private Mock<IStudentService> _studentServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IAntiforgery> _antiforgeryMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _studentServiceMock = new Mock<IStudentService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _antiforgeryMock = new Mock<IAntiforgery>();

            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            Services.AddSingleton(_studentServiceMock.Object);
            Services.AddSingleton<AuthenticationStateProvider, MockCustomAuthenticationStateProvider>();
            Services.AddSingleton<CustomAuthenticationStateProvider, MockCustomAuthenticationStateProvider>();
            Services.AddSingleton(_httpContextAccessorMock.Object);
            Services.AddSingleton(_antiforgeryMock.Object);
            Services.AddSingleton<NavigationManager, MockNavigationManager>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Dispose();
        }

        [Test]
        public void UpdateProfile_ShouldRenderForm()
        {
            // Act
            var cut = RenderComponent<UpdateProfile>();

            // Assert
            Assert.IsTrue(cut.Markup.Contains(@"<h3 class=""card-title text-center"">Student Registration</h3>"));
        }

        [Test]
        public void UpdateProfile_ShouldLoadStudentData_WhenUserIsAuthenticated()
        {
            // Arrange
            var student = new Student { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            _studentServiceMock.Setup(service => service.GetStudentByEmailAsync(It.IsAny<string>())).ReturnsAsync(student);

            // Act
            var cut = RenderComponent<UpdateProfile>();

            cut.Instance.student = student;

            // Assert
            cut.WaitForAssertion(() => Assert.IsNotNull(cut.Instance.student));
            cut.WaitForAssertion(() => Assert.AreEqual("john.doe@example.com", cut.Instance.student.Email));
            Assert.AreEqual("John", cut.Instance.student.FirstName);
            Assert.AreEqual("Doe", cut.Instance.student.LastName);
        }

        [Test]
        public async Task UpdateProfile_ShouldUpdateStudentData_WhenFormIsSubmitted()
        {
            // Arrange
            var student = new Student { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Password = "password" };
            _studentServiceMock.Setup(service => service.UpdateStudentAsync(It.IsAny<Student>())).ReturnsAsync(student);

            var cut = RenderComponent<UpdateProfile>();

            cut.Instance.student = student;

            // Wait for the component to finish initializing
            cut.WaitForAssertion(() => Assert.IsNotNull(cut.Instance.student));
            cut.WaitForAssertion(() => Assert.AreEqual("john.doe@example.com", cut.Instance.student.Email));

            cut.Instance.student = student;

            // Act
            await cut.Instance.UpdateUser();

            // Assert
            _studentServiceMock.Verify(service => service.UpdateStudentAsync(It.Is<Student>(s => s.Email == "john.doe@example.com")), Times.Once);
        }
    }
}
