using CourseEnrollmentApp.Core.Entities;

namespace CourseEnrollmentApp.Core.Interfaces.Repositories
{
    public interface ICourseRegistrationRepository
    {
        Task<IEnumerable<CourseRegistration?>> GetCourseRegistrationsByStudentIdAsync(int studentId);
        Task<IEnumerable<CourseRegistration?>> GetCourseNotRegisteredByStudentIdAsync(int studentId);
        Task<CourseRegistration?> AddCourseRegistrationAsync(CourseRegistration courseRegistration);
        Task<bool> RemoveCourseRegistrationAsync(CourseRegistration courseRegistration);
        Task<CourseRegistration?> GetCourseRegistrationByStudentIdAndCourseIdAsync(int studentId, int courseId);
    }
}
