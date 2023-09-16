using Microsoft.AspNetCore.Mvc;
using Studyzen.Lectures;
using StudyZen.Common;
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
    public async Task<IActionResult> CreateLecture(int courseId, [FromBody] CreateLectureRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        Lecture newLecture = _lectureService.AddLecture(courseId, request);
        return CreatedAtAction(nameof(GetLecture), new { lectureId = newLecture.Id }, newLecture);
    }

    [HttpGet]
    public async Task<IActionResult> ListLecturesByCourseId(int? courseId)
    {
        return Ok(_lectureService.GetLecturesByCourseId(courseId));
    }
}