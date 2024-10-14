using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollmentApp.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _dbContext.Courses.ToListAsync();
        }
    }
}
