using System.ComponentModel.DataAnnotations;

namespace CourseEnrollmentApp.Web.WASM.Models
{
    public class LoginDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "password is required")]
        public string? Password { get; set; }
    }
}
