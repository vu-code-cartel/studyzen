using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.FileMetadata;
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

    [HttpPost]
    [Route("csv")]
    [Consumes("multipart/form-data")]
    public IActionResult CreateFlashcardsFromCsv(IFormFile file, int flashcardSetId)
    {
        file.ThrowIfRequestArgumentNull(nameof(file));

        FileMetadata fileMetadata = new FileMetadata(file.FileName, file.ContentType, file.Length);

        using var stream = file.OpenReadStream();
        var flashcardsFromFile = _flashcardImporter.ImportFlashcardsFromCsv(stream, flashcardSetId, fileMetadata);
        var createdFlashcards = _flashcardService.CreateFlashcards(flashcardsFromFile);

        return Ok(createdFlashcards);
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
        var flashcards = _flashcardService.GetFlashcardsBySetId(flashcardSetId);
        return Ok(flashcards);
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
}