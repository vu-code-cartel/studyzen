using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.FlashCardSets.Requests;

namespace StudyZen.FlashCardSets;

[ApiController, Route("flashcardsets")]
public sealed class FlashCardSetsController : ControllerBase
{
    private readonly IFlashCardSetService _flashCardSetService;

    public FlashCardSetsController(IFlashCardSetService flashCardSetService)
    {
        _flashCardSetService = flashCardSetService;
    }

    [HttpPost]
    public IActionResult CreateFlashCardSet([FromBody] CreateFlashCardSetRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var createdFlashCardSet = _flashCardSetService.AddFlashCardSet(request);
        return CreatedAtAction(nameof(GetFlashCardSet), new
        {
            flashCardSetId = createdFlashCardSet.Id
        }, createdFlashCardSet);
    }

    [HttpGet("{flashCardSetId}")]
    public IActionResult GetFlashCardSet(int flashCardSetId)
    {
        var flashCardSet = _flashCardSetService.GetFlashCardSetById(flashCardSetId);
        return flashCardSet == null ? NotFound() : Ok(flashCardSet);
    }

    [HttpGet]
    [Route("all")]
    public IActionResult GetAllFlashCardSets()
    {
        return Ok(_flashCardSetService.GetAllFlashCardSets());
    }

    [HttpGet]
    public IActionResult GetFlashCardSetsByLectureId(int lectureId)
    {
        return Ok(_flashCardSetService.GetFlashCardSetsByLectureId(lectureId));
    }

    [HttpPatch("{flashCardSetId}")]
    public IActionResult UpdateFlashCardSetById(int flashCardSetId, [FromBody] UpdateFlashCardSetRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedFlashCardSet = _flashCardSetService.UpdateFlashCardSetById(flashCardSetId, request);
        return updatedFlashCardSet == null ? NotFound() : Ok(updatedFlashCardSet);
    }

    [HttpDelete("{flashCardSetId}")]
    public IActionResult DeleteFlashcardSet(int flashCardSetId)
    {
        _flashCardSetService.DeleteFlashCardSetById(flashCardSetId);
        return NoContent();
    }
}