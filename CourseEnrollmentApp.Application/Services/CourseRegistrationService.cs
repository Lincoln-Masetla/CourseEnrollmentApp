using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Core.Interfaces.Services;

namespace CourseEnrollmentApp.Application.Services
{
    public class CourseRegistrationService : ICourseRegistrationService
    {
        private readonly ICourseRegistrationRepository _courseRegistrationRepository;

        public CourseRegistrationService(ICourseRegistrationRepository courseRegistrationRepository)
        {
            _courseRegistrationRepository = courseRegistrationRepository;
        }

        public async Task<bool> RegisterCourseAsync(int studentId, int courseId)
        {
            var courseRegistration = new CourseRegistration
            {
                StudentId = studentId,
                CourseId = courseId
            };

            var result = await _courseRegistrationRepository.AddCourseRegistrationAsync(courseRegistration);

            return result != null;
        }

        public async Task<bool> DeregisterCourseAsync(int studentId, int courseId)
        {
            var courseRegistration = await _courseRegistrationRepository.GetCourseRegistrationByStudentIdAndCourseIdAsync(studentId, courseId);

            if (courseRegistration == null)
            {
                return false;
            }

            var result = await _courseRegistrationRepository.RemoveCourseRegistrationAsync(courseRegistration);

            return result;
        }
    }
}
