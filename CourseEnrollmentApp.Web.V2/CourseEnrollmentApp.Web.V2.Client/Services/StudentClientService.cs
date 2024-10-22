using System.Net.Http.Json;
using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Web.V2.Client.Services.Contract;

namespace CourseEnrollmentApp.Web.V2.Client.Services
{
    public class StudentClientService : IStudentClientService
    {
        private readonly HttpClient _httpClient;

        public StudentClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Student?> RegisterStudentAsync(Student student)
        {
            var response = await _httpClient.PostAsJsonAsync("api/students/register", student);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Student>();
            }
            return null;
        }

        public async Task<Student?> UpdateStudentAsync(Student student)
        {
            var response = await _httpClient.PostAsJsonAsync("api/students/update", student);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Student>();
            }
            return null;
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _httpClient.GetFromJsonAsync<Student>($"api/students/{email}");
        }

        public async Task<bool> RegisterCourseAsync(CourseRegistration registration)
        {
            var response = await _httpClient.PostAsJsonAsync("api/students/register-course", registration);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeregisterCourseAsync(CourseRegistration courseRegistration)
        {
            var response = await _httpClient.PostAsJsonAsync("api/students/courseregistrations/deregister-course", courseRegistration);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CourseRegistration?>> GetCourseRegistrationsByStudentIdAsync(int studentId)
        {
            var courses =  await _httpClient.GetFromJsonAsync<IEnumerable<CourseRegistration?>>($"api/courseregistrations/student/{studentId}");
            return courses ?? new List<CourseRegistration>();
        }

        public async Task<IEnumerable<CourseRegistration?>> GetCoursesNotRegisteredByStudentIdAsync(int studentId)
        {
            var courses = await _httpClient.GetFromJsonAsync<IEnumerable<CourseRegistration?>>($"api/courseregistrations/student/{studentId}/notregistered");
            return courses ?? new List<CourseRegistration>();
        }
    }
}
