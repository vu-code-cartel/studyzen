using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Api.Controllers;

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
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var newCourse = await _courseService.CreateCourse(request);
        return CreatedAtAction(nameof(GetCourse), new { courseId = newCourse.Id }, newCourse);
    }

    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetCourse(int courseId)
    {
        var course = await _courseService.GetCourseById(courseId);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var allCourses = await _courseService.GetAllCourses();
        return Ok(allCourses);
    }

    [HttpPatch]
    [Route("{courseId}")]
    public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] UpdateCourseDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var isSuccess = await _courseService.UpdateCourse(courseId, request);
        return isSuccess ? Ok() : BadRequest();
    }

    [HttpDelete]
    [Route("{courseId}")]
    public async Task<IActionResult> DeleteCourse(int courseId)
    {
        var isSuccess = await _courseService.DeleteCourse(courseId);
        return isSuccess ? Ok() : NotFound();
    }
}