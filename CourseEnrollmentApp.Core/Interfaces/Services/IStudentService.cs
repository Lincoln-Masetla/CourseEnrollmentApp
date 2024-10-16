using CourseEnrollmentApp.Core.Entities;

namespace CourseEnrollmentApp.Core.Interfaces.Services
{
    public interface IStudentService
    {
        Task<Student?> RegisterStudentAsync(Student student);
        Task<Student?> UpdateStudentAsync(Student student);
        Task<Student?> GetStudentByEmailAsync(string email);
    }
}
