using CourseEnrollmentApp.Core.Entities;

namespace CourseEnrollmentApp.Core.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> AddStudentAsync(Student student);
        Task<Student?> GetStudentByEmailAsync(string email);
    }
}
