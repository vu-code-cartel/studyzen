using Microsoft.AspNetCore.Mvc;
using Studyzen.Lectures;
using StudyZen.Common;
using StudyZen.Courses;
using StudyZen.Lectures.Forms;
using StudyZen.Lectures.Requests;

namespace StudyZen.Lectures;

[ApiController]
[Route("lectures")]
public sealed class LecturesController : ControllerBase
{
    private readonly ILectureService _lectureService;

    public LecturesController(ILectureService courseService)
    {
        _lectureService = courseService;
    }

    [HttpGet]
    [Route("{lectureId}")]
    public async Task<IActionResult> GetLecture(int lectureId)
    {
        var requestedLecture = _lectureService.GetLectureById(lectureId);
        return requestedLecture == null ? NotFound() : Ok(requestedLecture);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLecture([FromBody] CreateLectureRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var createdLecture = _lectureService.AddLecture(request);
        return CreatedAtAction(nameof(GetLecture), new { lectureId = createdLecture.Id }, createdLecture);
    }

    [HttpGet]
    public async Task<IActionResult> GetLecturesByCourseId(int? courseId)
    {
        return Ok(_lectureService.GetLecturesByCourseId(courseId));
    }

    [HttpPatch]
    [Route("{lectureId}")]
    public async Task<IActionResult> UpdateLecture(int lectureId, [FromBody] UpdateLectureRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedLecture = _lectureService.UpdateLectureById(lectureId, request);
        return updatedLecture == null ? NotFound() : Ok(updatedLecture);
    }

    [HttpDelete]
    [Route("{lectureId}")]
    public async Task<IActionResult> DeleteLecture(int lectureId)
    {
        _lectureService.DeleteLectureById(lectureId);
        return NoContent();
    }
}