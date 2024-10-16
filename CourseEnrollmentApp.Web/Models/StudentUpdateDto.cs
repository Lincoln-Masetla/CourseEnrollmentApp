using System.ComponentModel.DataAnnotations;

namespace CourseEnrollmentApp.Web.Models
{
    public class StudentUpdateDto : StudentDto
    {
        public int? Id { get; set; }
    }
}