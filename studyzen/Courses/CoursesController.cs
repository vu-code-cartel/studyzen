﻿using Microsoft.AspNetCore.Mvc;
using Studyzen.Common;
using Studyzen.Courses.Requests;

namespace Studyzen.Courses;

[Route("[controller]")]
public sealed class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));

        var courseId = _courseService.AddCourseAsync(request);

        return CreatedAtAction(nameof(GetCourse), new { courseId = courseId }, null);
    }

    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetCourse(int courseId)
    {
        return Ok(courseId);
    }
}