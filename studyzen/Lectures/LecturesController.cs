using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.Lectures.Requests;

namespace StudyZen.Lectures;

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
    public IActionResult CreateLecture([FromBody] CreateLectureRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var createdLecture = _lectureService.AddLecture(request);
        return CreatedAtAction(nameof(GetLecture), new { lectureId = createdLecture.Id }, createdLecture);
    }

    [HttpGet]
    public IActionResult GetLecturesByCourseId(int courseId)
    {
        return Ok(_lectureService.GetLecturesByCourseId(courseId));
    }

    [HttpGet]
    [Route("{lectureId}")]
    public IActionResult GetLecture(int lectureId)
    {
        var requestedLecture = _lectureService.GetLectureById(lectureId);
        return requestedLecture == null ? NotFound() : Ok(requestedLecture);
    }

    [HttpGet]
    [Route("all")]
    public IActionResult GetAllLectures()
    {
        return Ok(_lectureService.GetAllLectures());
    }

    [HttpPatch]
    [Route("{lectureId}")]
    public IActionResult UpdateLecture(int lectureId, [FromBody] UpdateLectureRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedLecture = _lectureService.UpdateLectureById(lectureId, request);
        return updatedLecture == null ? NotFound() : Ok(updatedLecture);
    }

    [HttpDelete]
    [Route("{lectureId}")]
    public IActionResult DeleteLecture(int lectureId)
    {
        _lectureService.DeleteLectureById(lectureId);
        return NoContent();
    }
}