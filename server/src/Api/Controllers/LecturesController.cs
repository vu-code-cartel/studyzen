using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using FluentValidation;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class LecturesController : ControllerBase
{
    private readonly ILectureService _lectureService;
    private readonly IValidator<CreateLectureDto> _createLectureValidator;
    private readonly IValidator<UpdateLectureDto> _updateLectureValidator;

    public LecturesController(ILectureService lectureService, IValidator<CreateLectureDto> createLectureValidator, IValidator<UpdateLectureDto> updateLectureValidator)
    {
        _lectureService = lectureService;
        _createLectureValidator = createLectureValidator;
        _updateLectureValidator = updateLectureValidator;
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
        var isSuccess = _lectureService.UpdateLecture(lectureId, request);
        return isSuccess ? Ok() : BadRequest();
    }

    [HttpDelete]
    [Route("{lectureId}")]
    public IActionResult DeleteLecture(int lectureId)
    {
        var isSuccess = _lectureService.DeleteLecture(lectureId);
        return isSuccess ? Ok() : NotFound();
    }
}