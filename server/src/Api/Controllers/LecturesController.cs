using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("lectures")]
public sealed class LecturesController : ControllerBase
{
    private readonly ILectureService _lectureService;

    public LecturesController(ILectureService lectureService)
    {
        _lectureService = lectureService;
    }

    [HttpPost]
    public IActionResult CreateLecture([FromBody] CreateLectureDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var createdLecture = _lectureService.CreateLecture(request);
        return CreatedAtAction(nameof(GetLecture), new { lectureId = createdLecture.Id }, createdLecture);
    }

    [HttpGet]
    public IActionResult GetLecturesByCourseId(int courseId)
    {
        var courseLectures = _lectureService.GetLecturesByCourseId(courseId);
        return Ok(courseLectures);
    }

    [HttpGet]
    [Route("{lectureId}")]
    public IActionResult GetLecture(int lectureId)
    {
        var lecture = _lectureService.GetLectureById(lectureId);
        return lecture is null ? NotFound() : Ok(lecture);
    }

    [HttpPatch]
    [Route("{lectureId}")]
    public IActionResult UpdateLecture(int lectureId, [FromBody] UpdateLectureDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedLecture = _lectureService.UpdateLecture(lectureId, request);
        return updatedLecture is null ? NotFound() : Ok(updatedLecture);
    }

    [HttpDelete]
    [Route("{lectureId}")]
    public IActionResult DeleteLecture(int lectureId)
    {
        _lectureService.DeleteLecture(lectureId);
        return NoContent();
    }
}