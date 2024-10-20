
using CourseEnrollmentApp.Application.Services;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CourseEnrollmentApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection service)
        {
            service.AddTransient<IStudentService, StudentService>();
            service.AddTransient<ICourseRegistrationService, CourseRegistrationService>();

            return service;
        }
    }
}
