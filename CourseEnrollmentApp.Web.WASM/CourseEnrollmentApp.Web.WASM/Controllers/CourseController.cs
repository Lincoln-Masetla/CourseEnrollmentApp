using CourseEnrollmentApp.Core.Interfaces.Repositories;
using CourseEnrollmentApp.Core.Interfaces.Services;
using CourseEnrollmentApp.Web.WASM.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

[Route("api/courses")]
[ApiController]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly ICourseRegistrationRepository _courseRegistrationRepository;
    private readonly ICourseRegistrationService _courseRegistrationService;
    private readonly ILogger<CourseController> _logger;

    public CourseController(ICourseRegistrationRepository courseRegistrationRepository, ICourseRegistrationService courseRegistrationService, ILogger<CourseController> logger)
    {
        _courseRegistrationRepository = courseRegistrationRepository;
        _courseRegistrationService = courseRegistrationService;
        _logger = logger;
    }

    [HttpGet("registered/{studentId}")]
    public async Task<IActionResult> GetRegisteredCourses(int studentId)
    {
        try
        {
            var enrolledCourses = await _courseRegistrationRepository.GetCourseRegistrationsByStudentIdAsync(studentId);
            var courses = enrolledCourses?.Select(cards => cards.Course).Select(c => new CourseDto { Id = c.Id, Name = c.Name}).ToList();

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting registered courses for studentId: {StudentId}", studentId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("not-registered/{studentId}")]
    public async Task<IActionResult> GetNotRegisteredCourses(int studentId)
    {
        try
        {
            var notRegisteredCourses = await _courseRegistrationRepository.GetCourseNotRegisteredByStudentIdAsync(studentId);
            return Ok(notRegisteredCourses?.Select(cr => cr.Course).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting not registered courses for studentId: {StudentId}", studentId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterForCourse([FromBody] EnrollCourseDto? enrollCourseDto)
    {
        if (enrollCourseDto == null)
        {
            return BadRequest("Invalid course registration data.");
        }

        if (!enrollCourseDto.CourseId.HasValue)
        {
            return BadRequest("Course ID is required.");
        }

        if (!enrollCourseDto.StudentId.HasValue)
        {
            return BadRequest("Student ID is required.");
        }

        try
        {
            var result = await _courseRegistrationService.RegisterCourseAsync(enrollCourseDto.StudentId.Value, enrollCourseDto.CourseId.Value);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to register for the course.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering for course with CourseId: {CourseId} and StudentId: {StudentId}", enrollCourseDto.CourseId, enrollCourseDto.StudentId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("deregister")]
    public async Task<IActionResult> DeregisterForCourse([FromBody] EnrollCourseDto? enrollCourseDto)
    {
        if (enrollCourseDto == null)
        {
            return BadRequest("Invalid course registration data.");
        }

        if (!enrollCourseDto.CourseId.HasValue)
        {
            return BadRequest("Course ID is required.");
        }

        if (!enrollCourseDto.StudentId.HasValue)
        {
            return BadRequest("Student ID is required.");
        }

        try
        {
            var result = await _courseRegistrationService.DeregisterCourseAsync(enrollCourseDto.StudentId.Value, enrollCourseDto.CourseId.Value);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to deregister from the course.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deregistering from course with CourseId: {CourseId} and StudentId: {StudentId}", enrollCourseDto.CourseId, enrollCourseDto.StudentId);
            return StatusCode(500, "Internal server error");
        }
    }
}
