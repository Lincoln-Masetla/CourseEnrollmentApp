using CourseEnrollmentApp.Core.Entities;
namespace CourseEnrollmentApp.Web.V2.Client.Services.Contract
{
    public interface IStudentClientService
    {
        Task<Student?> RegisterStudentAsync(Student student);
        Task<Student?> UpdateStudentAsync(Student student);
        Task<Student?> GetStudentByEmailAsync(string email);
        Task<bool> RegisterCourseAsync(CourseRegistration registration);
        Task<bool> DeregisterCourseAsync(CourseRegistration courseRegistration);
        Task<IEnumerable<CourseRegistration?>> GetCourseRegistrationsByStudentIdAsync(int studentId);
        Task<IEnumerable<CourseRegistration?>> GetCoursesNotRegisteredByStudentIdAsync(int studentId);
    }
}
