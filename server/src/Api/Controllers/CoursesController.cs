using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Lecturer")]
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
        return Ok(course);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var allCourses = await _courseService.GetAllCourses();
        return Ok(allCourses);
    }

    [HttpPatch]
    [Authorize(Roles = "Lecturer")]
    [Route("{courseId}")]
    public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] UpdateCourseDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        await _courseService.UpdateCourse(courseId, request);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Lecturer")]
    [Route("{courseId}")]
    public async Task<IActionResult> DeleteCourse(int courseId)
    {
        await _courseService.DeleteCourse(courseId);
        return Ok();
    }
}