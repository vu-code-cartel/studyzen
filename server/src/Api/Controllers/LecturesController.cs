using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class LecturesController : ControllerBase
{
    private readonly ILectureService _lectureService;

    public LecturesController(ILectureService lectureService)
    {
        _lectureService = lectureService;
    }

    [HttpPost]
    [Authorize(Roles = "Lecturer")]
    public async Task<IActionResult> CreateLecture([FromBody] CreateLectureDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var createdLecture = await _lectureService.CreateLecture(request);
        return CreatedAtAction(nameof(GetLecture), new { lectureId = createdLecture.Id }, createdLecture);
    }

    [HttpGet]
    [Authorize(Roles = "Lecturer, Student")]
    public async Task<IActionResult> GetLecturesByCourseId(int courseId)
    {
        var courseLectures = await _lectureService.GetLecturesByCourseId(courseId);
        return Ok(courseLectures);
    }

    [HttpGet]
    [Authorize(Roles = "Lecturer, Student")]
    [Route("{lectureId}")]
    public async Task<IActionResult> GetLecture(int lectureId)
    {
        var lecture = await _lectureService.GetLectureById(lectureId);
        return Ok(lecture);
    }

    [HttpPatch]
    [Authorize(Roles = "Lecturer")]
    [Route("{lectureId}")]
    public async Task<IActionResult> UpdateLecture(int lectureId, [FromBody] UpdateLectureDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        await _lectureService.UpdateLecture(lectureId, request);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Lecturer")]
    [Route("{lectureId}")]
    public async Task<IActionResult> DeleteLecture(int lectureId)
    {
        await _lectureService.DeleteLecture(lectureId);
        return Ok();
    }
}