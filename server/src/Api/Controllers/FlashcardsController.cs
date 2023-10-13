using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;
    private readonly FlashcardFileImporter _flashcardFileImporter;

    public FlashcardsController(IFlashcardService flashcardService, FlashcardFileImporter flashcardFileImporter)
    {
        _flashcardService = flashcardService;
        _flashcardFileImporter = flashcardFileImporter;
    }

    [HttpPost]
    public IActionResult CreateFlashcard([FromBody] CreateFlashcardDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var newFlashcard = _flashcardService.CreateFlashcard(request);
        return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = newFlashcard.Id }, newFlashcard);
    }

    [HttpGet]
    [Route("{flashcardId}")]
    public IActionResult GetFlashcard(int flashcardId)
    {
        var flashcard = _flashcardService.GetFlashcardById(flashcardId);
        return flashcard is null ? NotFound() : Ok(flashcard);
    }

    [HttpGet]
    public IActionResult GetFlashcardsBySetId(int flashcardSetId)
    {
        return Ok(_flashcardService.GetFlashcardsBySetId(flashcardSetId));
    }

    [HttpPatch]
    [Route("{flashcardId}")]
    public IActionResult UpdateFlashcardById(int flashcardId, [FromBody] UpdateFlashcardDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var isSuccess = _flashcardService.UpdateFlashcard(flashcardId, request);
        return isSuccess ? Ok() : BadRequest();
    }

    [HttpDelete("{flashcardId}")]
    public IActionResult DeleteFlashcard(int flashcardId)
    {
        var isSuccess = _flashcardService.DeleteFlashcard(flashcardId);
        return isSuccess ? Ok() : NotFound();
    }

    [HttpPost]
    [Route("csv")]
    [Consumes("multipart/form-data")]
    public IActionResult ImportFlashcardsFromCsv(IFormFile file, int flashcardSetId)
    {
        file = file.ThrowIfRequestArgumentNull(nameof(file));

        file.ImportFlashcardsFromCsvStream(_flashcardFileImporter, flashcardSetId);

        return Ok();
    }

}