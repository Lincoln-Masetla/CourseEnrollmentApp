
using CourseEnrollmentApp.Application.Services;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using Moq;

namespace CourseEnrollmentApp.Application.Tests.Services
{
    [TestFixture]
    public class CourseRegistrationServiceTests
    {
        private Mock<ICourseRegistrationRepository> _mockCourseRegistrationRepository;
        private CourseRegistrationService _courseRegistrationService;

        [SetUp]
        public void SetUp()
        {
            _mockCourseRegistrationRepository = new Mock<ICourseRegistrationRepository>();
            _courseRegistrationService = new CourseRegistrationService(_mockCourseRegistrationRepository.Object);
        }

        [Test]
        public async Task RegisterCourseAsync_ShouldReturnTrue_WhenCourseRegistrationIsSuccessful()
        {
            // Arrange
            var studentId = 1;
            var courseId = 1;
            var courseRegistration = new CourseRegistration { StudentId = studentId, CourseId = courseId };

            _mockCourseRegistrationRepository
                .Setup(repo => repo.AddCourseRegistrationAsync(It.IsAny<CourseRegistration>()))
                .ReturnsAsync(courseRegistration);

            // Act
            var result = await _courseRegistrationService.RegisterCourseAsync(studentId, courseId);

            // Assert
            Assert.IsTrue(result);
            _mockCourseRegistrationRepository.Verify(repo => repo.AddCourseRegistrationAsync(It.IsAny<CourseRegistration>()), Times.Once);
        }

        [Test]
        public async Task RegisterCourseAsync_ShouldReturnFalse_WhenCourseRegistrationFails()
        {
            // Arrange
            var studentId = 1;
            var courseId = 1;

            _mockCourseRegistrationRepository
                .Setup(repo => repo.AddCourseRegistrationAsync(It.IsAny<CourseRegistration>()))
                .ReturnsAsync((CourseRegistration?)null);

            // Act
            var result = await _courseRegistrationService.RegisterCourseAsync(studentId, courseId);

            // Assert
            Assert.IsFalse(result);
            _mockCourseRegistrationRepository.Verify(repo => repo.AddCourseRegistrationAsync(It.IsAny<CourseRegistration>()), Times.Once);
        }

        [Test]
        public async Task DeregisterCourseAsync_ShouldReturnTrue_WhenCourseDeregistrationIsSuccessful()
        {
            // Arrange
            var studentId = 1;
            var courseId = 1;
            var courseRegistration = new CourseRegistration { StudentId = studentId, CourseId = courseId };

            _mockCourseRegistrationRepository
                .Setup(repo => repo.GetCourseRegistrationByStudentIdAndCourseIdAsync(studentId, courseId))
                .ReturnsAsync(courseRegistration);

            _mockCourseRegistrationRepository
                .Setup(repo => repo.RemoveCourseRegistrationAsync(courseRegistration))
                .ReturnsAsync(true);

            // Act
            var result = await _courseRegistrationService.DeregisterCourseAsync(studentId, courseId);

            // Assert
            Assert.IsTrue(result);
            _mockCourseRegistrationRepository.Verify(repo => repo.GetCourseRegistrationByStudentIdAndCourseIdAsync(studentId, courseId), Times.Once);
            _mockCourseRegistrationRepository.Verify(repo => repo.RemoveCourseRegistrationAsync(courseRegistration), Times.Once);
        }

        [Test]
        public async Task DeregisterCourseAsync_ShouldReturnFalse_WhenCourseRegistrationDoesNotExist()
        {
            // Arrange
            var studentId = 1;
            var courseId = 1;

            _mockCourseRegistrationRepository
                .Setup(repo => repo.GetCourseRegistrationByStudentIdAndCourseIdAsync(studentId, courseId))
                .ReturnsAsync((CourseRegistration?)null);

            // Act
            var result = await _courseRegistrationService.DeregisterCourseAsync(studentId, courseId);

            // Assert
            Assert.IsFalse(result);
            _mockCourseRegistrationRepository.Verify(repo => repo.GetCourseRegistrationByStudentIdAndCourseIdAsync(studentId, courseId), Times.Once);
            _mockCourseRegistrationRepository.Verify(repo => repo.RemoveCourseRegistrationAsync(It.IsAny<CourseRegistration>()), Times.Never);
        }
    }
}

