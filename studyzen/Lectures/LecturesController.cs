using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Studyzen.Lectures;
using StudyZen.Common;
using StudyZen.Lectures.Requests;

namespace StudyZen.Lectures;

[ApiController]
[Route("courses/{courseId}/lectures")]
public sealed class LecturesController : ControllerBase
{
    [HttpGet]
    [Route("{lectureId}")]
    public async Task<IActionResult> GetLecture(int lectureId)
    {
        return Ok(lectureId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLecture(int courseId, [FromBody] CreateLectureRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        Lecture newLecture = new Lecture(courseId, request.Name, request.Content);
        return CreatedAtAction(nameof(GetLecture), new { courseId, lectureId = newLecture.Id }, newLecture);
    }
}