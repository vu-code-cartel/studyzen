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
        var newCourse = _courseService.AddCourse(request);
        return CreatedAtAction(nameof(GetCourse), new { courseId = newCourse.Id }, newCourse);
    }

    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetCourse(int courseId)
    {
        var fetchedCourse = _courseService.GetCourseById(courseId);
        return fetchedCourse == null ? NotFound() : Ok(fetchedCourse);
    }

    [HttpPatch]
    [Route("{courseId}")]
    public IActionResult UpdateCourse([FromBody] UpdateCourseRequest? request, int courseId)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedCourse = _courseService.UpdateCourse(request, courseId);
        return updatedCourse == null ? NotFound() : Ok(updatedCourse);
    }

    [HttpDelete]
    [Route("{courseId}")]
    public IActionResult DeleteCourse(int courseId)
    {
        _courseService.DeleteCourse(courseId);
        return NoContent();
    }

    [HttpGet]
    public IActionResult GetAllCourses()
    {
        var allCourses = _courseService.GetAllCourses();
        return Ok(allCourses);
    }

}