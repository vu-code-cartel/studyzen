using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.ValueObjects;
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
    public async Task<IActionResult> CreateFlashcard([FromBody] CreateFlashcardDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var newFlashcard = await _flashcardService.CreateFlashcard(request);
        return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = newFlashcard.Id }, newFlashcard);
    }

    [HttpPost]
    [Route("csv")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateFlashcardsFromCsv(IFormFile file, int flashcardSetId)
    {
        file.ThrowIfRequestArgumentNull(nameof(file));

        var fileMetadata = new FileMetadata(file.FileName, file.ContentType, file.Length);

        using var stream = file.OpenReadStream();

        var flashcardsFromFile = await _flashcardImporter.ImportFlashcardsFromCsv(stream, flashcardSetId, fileMetadata);
        var createdFlashcards = await _flashcardService.CreateFlashcards(flashcardsFromFile);

        return Ok(createdFlashcards);
    }

    [HttpGet]
    [Route("{flashcardId}")]
    public async Task<IActionResult> GetFlashcard(int flashcardId)
    {
        var flashcard = await _flashcardService.GetFlashcardById(flashcardId);
        return flashcard is null ? NotFound() : Ok(flashcard);
    }

    [HttpGet]
    public async Task<IActionResult> GetFlashcardsBySetId(int flashcardSetId)
    {
        return Ok(await _flashcardService.GetFlashcardsBySetId(flashcardSetId));
    }

    [HttpPatch]
    [Route("{flashcardId}")]
    public async Task<IActionResult> UpdateFlashcardById(int flashcardId, [FromBody] UpdateFlashcardDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var isSuccess = await _flashcardService.UpdateFlashcard(flashcardId, request);
        return isSuccess ? Ok() : BadRequest();
    }

    [HttpDelete("{flashcardId}")]
    public async Task<IActionResult> DeleteFlashcard(int flashcardId)
    {
        var isSuccess = await _flashcardService.DeleteFlashcard(flashcardId);
        return isSuccess ? Ok() : NotFound();
    }
}