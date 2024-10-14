using CourseEnrollmentApp.Core.Entities;

namespace CourseEnrollmentApp.Core.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllCoursesAsync();
        //Task<Course> GetCourseAsync(int courseId);
    }
}
