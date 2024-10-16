using CourseEnrollmentApp.Core.Entities;
using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Core.Interfaces.Services;

namespace CourseEnrollmentApp.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student?> RegisterStudentAsync(Student student)
        {
            
            if (await _studentRepository.GetStudentByEmailAsync(student.Email!) != null)
            {
                return null;
            }

            return await _studentRepository.AddStudentAsync(student);
        }

        public async Task<Student?> UpdateStudentAsync(Student student)
        {
            if (await _studentRepository.GetStudentByEmailAsync(student.Email!) != null)
            {
                return null;
            }
            return await _studentRepository.UpdateStudentAsync(student) ?? new Student();
        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            return await _studentRepository.GetStudentByEmailAsync(email);
        }

    }
}
