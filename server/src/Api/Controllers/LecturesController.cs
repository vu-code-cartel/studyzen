using System.Security.Claims;
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
    private readonly IUserContextService _userContextService;

    public LecturesController(ILectureService lectureService, IUserContextService userContextService)
    {
        _lectureService = lectureService;
        _userContextService = userContextService;
    }

    [HttpPost]
    [Authorize(Roles = "Lecturer")]
    public async Task<IActionResult> CreateLecture([FromBody] CreateLectureDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        _userContextService.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var createdLecture = await _lectureService.CreateLecture(request, _userContextService.ApplicationUserId!);
        return CreatedAtAction(nameof(GetLecture), new { lectureId = createdLecture.Id }, createdLecture);
    }

    [HttpGet]
    public async Task<IActionResult> GetLecturesByCourseId(int courseId)
    {
        var courseLectures = await _lectureService.GetLecturesByCourseId(courseId);
        return Ok(courseLectures);
    }

    [HttpGet]
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
        _userContextService.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _lectureService.UpdateLecture(lectureId, request, _userContextService.ApplicationUserId!);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Lecturer")]
    [Route("{lectureId}")]
    public async Task<IActionResult> DeleteLecture(int lectureId)
    {
        _userContextService.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        await _lectureService.DeleteLecture(lectureId, _userContextService.ApplicationUserId!);
        return Ok();
    }
}