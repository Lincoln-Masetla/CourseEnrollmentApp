using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Infrastructure.Data;
using CourseEnrollmentApp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CourseEnrollmentApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddTransient<IStudentRepository, StudentRepository>();
            service.AddTransient<ICourseRegistrationRepository, CourseRegistrationRepository>();

            service.AddScoped(_ => InMemoryDatabase.GetOptions());
            service.AddScoped<ApplicationDbContext>();

            return service;
        }

        public static async Task<IServiceProvider> AddInfrastructureAsync(this IServiceProvider provider)
        {
            await InitializeDatabase(provider);
            return provider;
        }

        private static async Task InitializeDatabase(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                InMemoryDatabase.SeedData(dbContext);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
