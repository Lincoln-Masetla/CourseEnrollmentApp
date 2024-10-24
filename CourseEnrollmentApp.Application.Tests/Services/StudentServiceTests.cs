using CourseEnrollmentApp.Application.Services;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using Moq;

namespace CourseEnrollmentApp.Application.Tests.Services
{
    namespace CourseEnrollmentApp.Tests.Services
    {
        [TestFixture]
        public class StudentServiceTests
        {
            private Mock<IStudentRepository> _mockStudentRepository;
            private StudentService _studentService;

            [SetUp]
            public void SetUp()
            {
                _mockStudentRepository = new Mock<IStudentRepository>();
                _studentService = new StudentService(_mockStudentRepository.Object);
            }

            [Test]
            public async Task RegisterStudentAsync_ShouldReturnStudent_WhenRegistrationIsSuccessful()
            {
                // Arrange
                var student = new Student { Email = "test@example.com" };

                _mockStudentRepository
                    .Setup(repo => repo.GetStudentByEmailAsync(student.Email))
                    .ReturnsAsync((Student?)null);

                _mockStudentRepository
                    .Setup(repo => repo.AddStudentAsync(student))
                    .ReturnsAsync(student);

                // Act
                var result = await _studentService.RegisterStudentAsync(student);

                // Assert
                Assert.IsNotNull(result);
                Assert.That(result.Email, Is.EqualTo(student.Email));
                _mockStudentRepository.Verify(repo => repo.GetStudentByEmailAsync(student.Email), Times.Once);
                _mockStudentRepository.Verify(repo => repo.AddStudentAsync(student), Times.Once);
            }

            [Test]
            public async Task RegisterStudentAsync_ShouldReturnNull_WhenStudentAlreadyExists()
            {
                // Arrange
                var student = new Student { Email = "test@example.com" };

                _mockStudentRepository
                    .Setup(repo => repo.GetStudentByEmailAsync(student.Email))
                    .ReturnsAsync(student);

                // Act
                var result = await _studentService.RegisterStudentAsync(student);

                // Assert
                Assert.IsNull(result);
                _mockStudentRepository.Verify(repo => repo.GetStudentByEmailAsync(student.Email), Times.Once);
                _mockStudentRepository.Verify(repo => repo.AddStudentAsync(It.IsAny<Student>()), Times.Never);
            }

            [Test]
            public async Task UpdateStudentAsync_ShouldReturnStudent_WhenUpdateIsSuccessful()
            {
                // Arrange
                var student = new Student { Email = "test@example.com" };

                _mockStudentRepository
                    .Setup(repo => repo.GetStudentByEmailAsync(student.Email))
                    .ReturnsAsync(student);

                _mockStudentRepository
                    .Setup(repo => repo.UpdateStudentAsync(student))
                    .ReturnsAsync(student);

                // Act
                var result = await _studentService.UpdateStudentAsync(student);

                // Assert
                Assert.IsNotNull(result);
                Assert.That(result.Email, Is.EqualTo(student.Email));
                _mockStudentRepository.Verify(repo => repo.GetStudentByEmailAsync(student.Email), Times.Once);
                _mockStudentRepository.Verify(repo => repo.UpdateStudentAsync(student), Times.Once);
            }

            [Test]
            public async Task UpdateStudentAsync_ShouldReturnNull_WhenStudentAlreadyExists()
            {
                // Arrange
                var student = new Student { Email = "test@example.com" };

                _mockStudentRepository
                    .Setup(repo => repo.GetStudentByEmailAsync(student.Email))
                    .ReturnsAsync((Student?)null);

                // Act
                var result = await _studentService.UpdateStudentAsync(student);

                // Assert
                Assert.IsNull(result);
                _mockStudentRepository.Verify(repo => repo.GetStudentByEmailAsync(student.Email), Times.Once);
                _mockStudentRepository.Verify(repo => repo.UpdateStudentAsync(It.IsAny<Student>()), Times.Never);
            }

            [Test]
            public async Task GetStudentByEmailAsync_ShouldReturnStudent_WhenStudentExists()
            {
                // Arrange
                var email = "test@example.com";
                var student = new Student { Email = email };

                _mockStudentRepository
                    .Setup(repo => repo.GetStudentByEmailAsync(email))
                    .ReturnsAsync(student);

                // Act
                var result = await _studentService.GetStudentByEmailAsync(email);

                // Assert
                Assert.IsNotNull(result);
                Assert.That(result.Email, Is.EqualTo(email));
                _mockStudentRepository.Verify(repo => repo.GetStudentByEmailAsync(email), Times.Once);
            }

            [Test]
            public async Task GetStudentByEmailAsync_ShouldReturnNull_WhenStudentDoesNotExist()
            {
                // Arrange
                var email = "test@example.com";

                _mockStudentRepository
                    .Setup(repo => repo.GetStudentByEmailAsync(email))
                    .ReturnsAsync((Student?)null);

                // Act
                var result = await _studentService.GetStudentByEmailAsync(email);

                // Assert
                Assert.IsNull(result);
                _mockStudentRepository.Verify(repo => repo.GetStudentByEmailAsync(email), Times.Once);
            }
        }
    }

}
