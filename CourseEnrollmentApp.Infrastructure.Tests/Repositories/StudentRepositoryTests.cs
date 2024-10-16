using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Infrastructure.Data;
using CourseEnrollmentApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollmentApp.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class StudentRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private StudentRepository _studentRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _studentRepository = new StudentRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddStudentAsync_ShouldAddStudent()
        {
            // Arrange
            var student = new Student { Email = "test@example.com", FirstName = "John", LastName = "Doe" };

            // Act
            var result = await _studentRepository.AddStudentAsync(student);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo(student.Email));
            Assert.That(_dbContext.Students.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateStudentAsync_ShouldUpdateStudent()
        {
            // Arrange
            var student = new Student { Email = "test@example.com", FirstName = "John", LastName = "Doe" };
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();

            student.FirstName = "Jane";

            // Act
            var result = await _studentRepository.UpdateStudentAsync(student);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.FirstName, Is.EqualTo("Jane"));
            Assert.That(_dbContext.Students.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetStudentByEmailAsync_ShouldReturnStudent()
        {
            // Arrange
            var email = "test@example.com";
            var student = new Student { Email = email, FirstName = "John", LastName = "Doe" };
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _studentRepository.GetStudentByEmailAsync(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo(email));
        }
    }
}
