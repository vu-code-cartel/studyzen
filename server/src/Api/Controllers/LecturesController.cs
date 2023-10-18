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
    public async Task<IActionResult> CreateLecture([FromBody] CreateLectureDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var createdLecture = await _lectureService.CreateLecture(request);
        return CreatedAtAction(nameof(GetLecture), new { lectureId = createdLecture.Id }, createdLecture);
    }

    [HttpGet]
    public async Task<IActionResult> GetLecturesByCourseId(int courseId)
    {
        var lectures = await _lectureService.GetLecturesByCourseId(courseId);
        return Ok(lectures);
    }

    [HttpGet]
    [Route("{lectureId}")]
    public async Task<IActionResult> GetLecture(int lectureId)
    {
        var lecture = await _lectureService.GetLectureById(lectureId);
        return lecture is null ? NotFound() : Ok(lecture);
    }

    [HttpPatch]
    [Route("{lectureId}")]
    public async Task<IActionResult> UpdateLecture(int lectureId, [FromBody] UpdateLectureDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var isSuccess = await _lectureService.UpdateLecture(lectureId, request);
        return isSuccess ? Ok() : BadRequest();
    }

    [HttpDelete]
    [Route("{lectureId}")]
    public async Task<IActionResult> DeleteLecture(int lectureId)
    {
        var isSuccess = await _lectureService.DeleteLecture(lectureId);
        return isSuccess ? Ok() : NotFound();
    }
}