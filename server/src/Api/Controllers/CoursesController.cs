using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using FluentValidation;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IValidator<CreateCourseDto> _createCourseValidator;
    private readonly IValidator<UpdateCourseDto> _updateCourseValidator;

    public CoursesController(ICourseService courseService, IValidator<CreateCourseDto> createCourseValidator, IValidator<UpdateCourseDto> updateCourseValidator)
    {
        _courseService = courseService;
        _createCourseValidator = createCourseValidator;
        _updateCourseValidator = updateCourseValidator;
    }

    [HttpPost]
    public IActionResult CreateCourse([FromBody] CreateCourseDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var validationResult = _createCourseValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        else
        {
            var newCourse = _courseService.CreateCourse(request);
            return CreatedAtAction(nameof(GetCourse), new { courseId = newCourse.Id }, newCourse);
        }
    }

    [HttpGet]
    [Route("{courseId}")]
    public IActionResult GetCourse(int courseId)
    {
        var course = _courseService.GetCourseById(courseId);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpGet]
    public IActionResult GetAllCourses()
    {
        var allCourses = _courseService.GetAllCourses();
        return Ok(allCourses);
    }

    [HttpPatch]
    [Route("{courseId}")]
    public IActionResult UpdateCourse(int courseId, [FromBody] UpdateCourseDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var validationResult = _updateCourseValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        else
        {
            try
            {
                var updatedCourse = _courseService.UpdateCourse(courseId, request);
                return updatedCourse is null ? NotFound() : Ok(updatedCourse);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
        }
    }

    [HttpDelete]
    [Route("{courseId}")]
    public IActionResult DeleteCourse(int courseId)
    {
        _courseService.DeleteCourse(courseId);
        return NoContent();
    }
}