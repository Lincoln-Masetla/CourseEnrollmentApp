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

        public async Task<Student?> AddStudentAsync(Student student)
        {
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> UpdateStudentAsync(Student student)
        {
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _dbContext.Students
                .AsNoTracking()
                .Include(s => s.CourseRegistrations)
                .FirstOrDefaultAsync(s => s.Email == email);
        }
    }
}
