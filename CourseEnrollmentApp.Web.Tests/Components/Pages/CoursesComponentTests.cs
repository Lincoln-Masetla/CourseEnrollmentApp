using Bunit;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Core.Interfaces.Services;
using CourseEnrollmentApp.Web.Components.Pages;
using CourseEnrollmentApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseEnrollmentApp.Web.Tests.Components.Pages
{
    [TestFixture]
    public class CoursesComponentTests : Bunit.TestContext
    {
        private Mock<ICourseRegistrationRepository> _courseRegistrationRepositoryMock;
        private Mock<ICourseRegistrationService> _courseRegistrationServiceMock;
        private Mock<IStudentService> _studentServiceMock;
        private CustomAuthenticationStateProvider _authStateProvider;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private DefaultHttpContext _httpContext;
        private Mock<IServiceProvider> _serviceProviderMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _courseRegistrationRepositoryMock = new Mock<ICourseRegistrationRepository>();
            _courseRegistrationServiceMock = new Mock<ICourseRegistrationService>();
            _studentServiceMock = new Mock<IStudentService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _serviceProviderMock = new Mock<IServiceProvider>();

            _httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProviderMock.Object
            };
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(_httpContext);

            _authStateProvider = new MockCustomAuthenticationStateProvider(_httpContextAccessorMock.Object);

            Services.AddSingleton(_courseRegistrationRepositoryMock.Object);
            Services.AddSingleton(_courseRegistrationServiceMock.Object);
            Services.AddSingleton(_studentServiceMock.Object);
            Services.AddSingleton<AuthenticationStateProvider>(_authStateProvider);
            Services.AddSingleton<CustomAuthenticationStateProvider>(_authStateProvider);
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
        public void CoursesComponent_ShouldRenderWithoutErrors()
        {
            // Act
            var cut = RenderComponent<Courses>();

            // Assert
            Assert.IsNotNull(cut);
        }

        [Test]
        public async Task CoursesComponent_ShouldRenderCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = 1, Name = "Course 1" },
                new Course { Id = 2, Name = "Course 2" }
            };

            var courseRegistrations = courses.Select(c => new CourseRegistration { Course = c }).ToList();

            _courseRegistrationRepositoryMock
                .Setup(repo => repo.GetCourseNotRegisteredByStudentIdAsync(It.IsAny<int>()))
                .ReturnsAsync(courseRegistrations);

            // Act
            var cut = RenderComponent<Courses>();
            await cut.InvokeAsync(() => cut.Instance.GetCourses());

            // Assert
            cut.Render();
            foreach (var course in courses)
            {
                if (course.Name != null)
                {
                    Assert.IsTrue(cut.Markup.Contains(course.Name));
                }
            }
        }

        [Test]
        public void CoursesComponent_ShouldRenderLoadingMessage()
        {
            //Arrange
            _courseRegistrationRepositoryMock
                .Setup(repo => repo.GetCourseNotRegisteredByStudentIdAsync(It.IsAny<int>()))
                .ReturnsAsync((List<CourseRegistration>?)null);

            // Act
            var cut = RenderComponent<Courses>();

            // Assert
            cut.Render();
            Assert.IsTrue(cut.Markup.Contains("Loading courses..."));
        }

        [Test]
        public async Task CoursesComponent_ShouldHandleRegistrationFailure()
        {
            // Arrange
            var course = new Course { Id = 1, Name = "Course 1" };
            _courseRegistrationServiceMock
                .Setup(service => service.RegisterCourseAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var cut = RenderComponent<Courses>();
            await cut.InvokeAsync(() => cut.Instance.RegisterForCourse(course));

            // Assert
            cut.Render();
            Assert.IsTrue(cut.Markup.Contains("Failed to register for the course. Please try again."));
        }

        [Test]
        public async Task CoursesComponent_ShouldHandleRegistrationSuccess()
        {
            // Arrange
            var course = new Course { Id = 1, Name = "Course 1" };
            _courseRegistrationServiceMock
                .Setup(service => service.RegisterCourseAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var courses = new List<Course>
            {
                new Course { Id = 1, Name = "Course 1" },
                new Course { Id = 2, Name = "Course 2" }
            };

            var courseRegistrations = courses.Select(c => new CourseRegistration { Course = c }).ToList();

            _courseRegistrationRepositoryMock
                .Setup(repo => repo.GetCourseNotRegisteredByStudentIdAsync(It.IsAny<int>()))
                .ReturnsAsync(courseRegistrations);

            // Act
            var cut = RenderComponent<Courses>();
            await cut.InvokeAsync(() => cut.Instance.RegisterForCourse(course));

            // Assert
            cut.Render();
            foreach (var c in courses)
            {
                if (c.Name != null)
                {
                    Assert.IsTrue(cut.Markup.Contains(c.Name));
                }
            }
        }
    }
}
