using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollmentApp.Infrastructure.Repositories
{
    public class CourseRegistrationRepository : ICourseRegistrationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseRegistrationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CourseRegistration?> AddCourseRegistrationAsync(CourseRegistration courseRegistration)
        {
            _dbContext.CourseRegistrations.Add(courseRegistration);
            await _dbContext.SaveChangesAsync();
            return courseRegistration;
        }

        public async Task<bool> RemoveCourseRegistrationAsync(CourseRegistration courseRegistration)
        {
            _dbContext.CourseRegistrations.Remove(courseRegistration);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<CourseRegistration?>> GetCourseRegistrationsByStudentIdAsync(int studentId)
        {
            return await _dbContext.CourseRegistrations
                .Where(cr => cr.StudentId == studentId)
                .Include(cr => cr.Course)
                .ToListAsync();
        }

        public async Task<CourseRegistration?> GetCourseRegistrationByStudentIdAndCourseIdAsync(int studentId, int courseId)
        {
            return await _dbContext.CourseRegistrations
                .FirstOrDefaultAsync(cr => cr.StudentId == studentId && cr.CourseId == courseId);
        }

        public async Task<IEnumerable<CourseRegistration?>> GetCourseNotRegisteredByStudentIdAsync(int studentId)
        {
            return await _dbContext.Courses
                .Where(c => !c.CourseRegistrations!.Any(cr => cr.StudentId == studentId))
                .Select(c => new CourseRegistration
                {
                    CourseId = c.Id,
                    Course = c
                }).ToListAsync();
        }
    }
}
