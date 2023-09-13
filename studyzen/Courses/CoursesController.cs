using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.Courses.Requests;

namespace StudyZen.Courses;

[ApiController]
[Route("[controller]")]
public sealed class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    public IActionResult CreateCourse([FromBody] CreateCourseRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));

        var courseId = _courseService.AddCourse(request);

        return CreatedAtAction(nameof(GetCourse), new { courseId = courseId }, null);
    }

    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetCourse(int courseId)
    {
        return Ok(courseId);
    }
}