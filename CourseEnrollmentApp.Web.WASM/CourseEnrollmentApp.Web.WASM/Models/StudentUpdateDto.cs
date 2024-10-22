using System.ComponentModel.DataAnnotations;

namespace CourseEnrollmentApp.Web.WASM.Models
{
    public class StudentUpdateDto : StudentDto
    {
        public int? Id { get; set; }
    }
}