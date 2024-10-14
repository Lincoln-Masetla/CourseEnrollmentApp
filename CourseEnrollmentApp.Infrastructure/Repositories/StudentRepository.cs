using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollmentApp.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _dbContext.Students.Add(student);
            var results = await _dbContext.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _dbContext.Students
                .Include(c => c.CourseRegistrations)
                .FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
