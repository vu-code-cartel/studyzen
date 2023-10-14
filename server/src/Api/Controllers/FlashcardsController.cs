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
    private readonly IFlashcardImporter _flashcardImporter;

    public FlashcardsController(IFlashcardService flashcardService, IFlashcardImporter flashcardImporter)
    {
        _flashcardService = flashcardService;
        _flashcardImporter = flashcardImporter;
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

        var flashcardDtos = file.ImportFlashcardsFromCsvStream(_flashcardImporter, flashcardSetId);

        var createdFlashcards = _flashcardService.CreateFlashcards(flashcardDtos);

        return Ok(createdFlashcards);
    }

}