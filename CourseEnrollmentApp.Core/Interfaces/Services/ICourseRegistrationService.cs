namespace CourseEnrollmentApp.Core.Interfaces.Services
{
    public interface ICourseRegistrationService
    {
        Task<bool> RegisterCourseAsync(int studentId, int courseId);
        Task<bool> DeregisterCourseAsync(int studentId, int courseId);
    }
}
