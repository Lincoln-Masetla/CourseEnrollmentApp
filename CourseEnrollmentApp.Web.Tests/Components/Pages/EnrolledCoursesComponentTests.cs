using Bunit;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Core.Interfaces.Services;
using CourseEnrollmentApp.Web.Components.Pages;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CourseEnrollmentApp.Web.Tests.Components.Pages
{
    [TestFixture]
    public class EnrolledCoursesComponentTests : Bunit.TestContext
    {
        private Mock<ICourseRegistrationRepository> _courseRegistrationRepositoryMock;
        private Mock<ICourseRegistrationService> _courseRegistrationServiceMock;

        [OneTimeSetUp]
        public void SetUp()
        {
            _courseRegistrationRepositoryMock = new Mock<ICourseRegistrationRepository>();
            _courseRegistrationServiceMock = new Mock<ICourseRegistrationService>();

            Services.AddSingleton(_courseRegistrationRepositoryMock.Object);
            Services.AddSingleton(_courseRegistrationServiceMock.Object);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Dispose();
        }

        [Test]
        public void EnrolledCoursesComponent_ShouldRenderLoadingMessageInitially()
        {
            // Arrange
            _courseRegistrationRepositoryMock
                .Setup(repo => repo.GetCourseRegistrationsByStudentIdAsync(It.IsAny<int>()))
                .ReturnsAsync((List<CourseRegistration>)null);

            // Act
            var cut = RenderComponent<EnrolledCourses>();

            // Assert
            cut.Markup.Contains(@"<p>Loading courses...</p>");
            Assert.IsTrue(cut.Markup.Contains(@"<p>Loading courses...</p>"));
        }

        [Test]
        public async Task EnrolledCoursesComponent_ShouldRenderCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { Id = 1, Name = "Course 1" },
                new Course { Id = 2, Name = "Course 2" }
            };

            var courseRegistrations = courses.Select(c => new CourseRegistration { Course = c }).ToList();

            _courseRegistrationRepositoryMock
                .Setup(repo => repo.GetCourseRegistrationsByStudentIdAsync(It.IsAny<int>()))
                .ReturnsAsync(courseRegistrations);

            // Act
            var cut = RenderComponent<EnrolledCourses>();
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
        public async Task EnrolledCoursesComponent_ShouldDeregisterForCourse()
        {
            // Arrange
            var course = new Course { Id = 1, Name = "Course 1" };
            var courseRegistrations = new List<CourseRegistration> { new CourseRegistration { Course = course } };

            _courseRegistrationRepositoryMock
                .Setup(repo => repo.GetCourseRegistrationsByStudentIdAsync(It.IsAny<int>()))
                .ReturnsAsync(courseRegistrations);

            _courseRegistrationServiceMock
                .Setup(service => service.DeregisterCourseAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var cut = RenderComponent<EnrolledCourses>();
            await cut.InvokeAsync(() => cut.Instance.DeRegisterForCourse(course));

            // Assert
            _courseRegistrationServiceMock.Verify(service => service.DeregisterCourseAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _courseRegistrationRepositoryMock.Verify(repo => repo.GetCourseRegistrationsByStudentIdAsync(It.IsAny<int>()), Times.Exactly(2));
        }
    }
}
