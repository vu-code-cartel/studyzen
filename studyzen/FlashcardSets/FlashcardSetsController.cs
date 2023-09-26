using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.FlashcardSets.Requests;

namespace StudyZen.FlashcardSets;

[ApiController]
[Route("flashcardsets")]
public sealed class FlashcardSetsController : ControllerBase
{
    private readonly IFlashcardSetService _flashcardSetService;

    public FlashcardSetsController(IFlashcardSetService flashcardSetService)
    {
        _flashcardSetService = flashcardSetService;
    }

    [HttpPost]
    public IActionResult CreateFlashcardSet([FromBody] CreateFlashcardSetRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var createdFlashcardSet = _flashcardSetService.CreateFlashcardSet(request);
        return CreatedAtAction(nameof(GetFlashcardSet), new
        {
            flashcardSetId = createdFlashcardSet.Id
        }, createdFlashcardSet);
    }

    [HttpGet("{flashcardSetId}")]
    public IActionResult GetFlashcardSet(int flashcardSetId)
    {
        var flashcardSet = _flashcardSetService.GetFlashcardSetById(flashcardSetId);
        return flashcardSet == null ? NotFound() : Ok(flashcardSet);
    }

    [HttpGet]
    [Route("all")]
    public IActionResult GetAllFlashcardSets()
    {
        return Ok(_flashcardSetService.GetAllFlashcardSets());
    }

    [HttpGet]
    public IActionResult GetFlashcardSetsByLectureId(int lectureId)
    {
        return Ok(_flashcardSetService.GetFlashcardSetsByLectureId(lectureId));
    }

    [HttpPatch("{flashcardSetId}")]
    public IActionResult UpdateFlashcardSetById(int flashcardSetId, [FromBody] UpdateFlashcardSetRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedFlashcardSet = _flashcardSetService.UpdateFlashcardSet(flashcardSetId, request);
        return updatedFlashcardSet == null ? NotFound() : Ok(updatedFlashcardSet);
    }

    [HttpDelete("{flashcardSetId}")]
    public IActionResult DeleteFlashcardSet(int flashcardSetId)
    {
        _flashcardSetService.DeleteFlashcardSet(flashcardSetId);
        return NoContent();
    }
}