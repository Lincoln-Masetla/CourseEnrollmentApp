using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Infrastructure.Data;
using CourseEnrollmentApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollmentApp.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class CourseRegistrationRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private CourseRegistrationRepository _courseRegistrationRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _courseRegistrationRepository = new CourseRegistrationRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddCourseRegistrationAsync_ShouldAddCourseRegistration()
        {
            // Arrange
            var courseRegistration = new CourseRegistration { StudentId = 1, CourseId = 1 };

            // Act
            var result = await _courseRegistrationRepository.AddCourseRegistrationAsync(courseRegistration);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StudentId, Is.EqualTo(courseRegistration.StudentId));
            Assert.That(result.CourseId, Is.EqualTo(courseRegistration.CourseId));
            Assert.That(_dbContext.CourseRegistrations.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task RemoveCourseRegistrationAsync_ShouldRemoveCourseRegistration()
        {
            // Arrange
            var courseRegistration = new CourseRegistration { StudentId = 1, CourseId = 1 };
            _dbContext.CourseRegistrations.Add(courseRegistration);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _courseRegistrationRepository.RemoveCourseRegistrationAsync(courseRegistration);

            // Assert
            Assert.IsTrue(result);
            Assert.That(_dbContext.CourseRegistrations.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetCourseRegistrationsByStudentIdAsync_ShouldReturnRegistrations()
        {
            // Arrange
            var studentId = 1;
            var student = new Student
            {
                Id = studentId,
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var courseRegistrations = new List<CourseRegistration>
            {
                new CourseRegistration
                {
                    StudentId = studentId,
                    CourseId = 1,
                    Course = new Course { Id = 1, Name = "Course 1" },
                    Student = student
                },
                new CourseRegistration
                {
                    StudentId = studentId,
                    CourseId = 2,
                    Course = new Course { Id = 2, Name = "Course 2" },
                    Student = student
                }
            };

            _dbContext.CourseRegistrations.AddRange(courseRegistrations);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _courseRegistrationRepository.GetCourseRegistrationsByStudentIdAsync(studentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }



        [Test]
        public async Task GetCourseRegistrationByStudentIdAndCourseIdAsync_ShouldReturnRegistration()
        {
            // Arrange
            var studentId = 1;
            var courseId = 1;
            var courseRegistration = new CourseRegistration { StudentId = studentId, CourseId = courseId };
            _dbContext.CourseRegistrations.Add(courseRegistration);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _courseRegistrationRepository.GetCourseRegistrationByStudentIdAndCourseIdAsync(studentId, courseId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StudentId, Is.EqualTo(studentId));
            Assert.That(result.CourseId, Is.EqualTo(courseId));
        }

        [Test]
        public async Task GetCourseNotRegisteredByStudentIdAsync_ShouldReturnCourses()
        {
            // Arrange
            var studentId = 1;
            var courses = new List<Course>
        {
            new Course { Id = 1, Name = "Course 1" },
            new Course { Id = 2, Name = "Course 2" }
        };
            _dbContext.Courses.AddRange(courses);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _courseRegistrationRepository.GetCourseNotRegisteredByStudentIdAsync(studentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
