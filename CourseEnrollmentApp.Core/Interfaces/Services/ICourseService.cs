using CourseEnrollmentApp.Core.Entities;

namespace CourseEnrollmentApp.Core.Interfaces.Services
{
    public interface ICourseService
    {
        Task<List<Course>> GetAllCoursesAsync();
    }
}
