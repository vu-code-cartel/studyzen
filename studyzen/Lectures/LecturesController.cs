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
        Lecture? requestedLecture = _lectureService.GetLectureById(lectureId);
        if (requestedLecture == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(requestedLecture);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateLecture([FromForm] CreateLectureForm form, [FromBody] CreateLectureRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        Lecture createdLecture = _lectureService.AddLecture(request);
        return CreatedAtAction(nameof(GetLecture), new { lectureId = createdLecture.Id }, createdLecture);
    }

    [HttpGet]
    public async Task<IActionResult> ListLecturesByCourseId(int? courseId)
    {
        return Ok(_lectureService.GetLecturesByCourseId(courseId));
    }

    [HttpPatch]
    [Route("{lectureId}")]
    public async Task<IActionResult> UpdateLecture(int lectureId, [FromBody] UpdateLectureRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        Lecture? updatedLecture = _lectureService.UpdateLectureById(lectureId, request.Name, request.Content);
        if (updatedLecture != null)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    [Route("{lectureId}")]
    public async Task<IActionResult> DeleteLecture(int lectureId)
    {
        _lectureService.DeleteLectureById(lectureId);
        return NoContent();
    }
}