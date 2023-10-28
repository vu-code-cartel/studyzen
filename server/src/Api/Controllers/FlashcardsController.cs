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

        await using var stream = file.OpenReadStream();

        var fileMetadata = new FileMetadata(file.FileName, file.Length);
        var flashcardsFromFile = await _flashcardImporter.ImportFlashcardsFromCsv(stream, flashcardSetId, fileMetadata);
        var createdFlashcards = await _flashcardService.CreateFlashcards(flashcardsFromFile);

        return Ok(createdFlashcards);
    }

    [HttpGet]
    [Route("{flashcardId}")]
    public async Task<IActionResult> GetFlashcard(int flashcardId)
    {
        var flashcard = await _flashcardService.GetFlashcardById(flashcardId);
        return Ok(flashcard);
    }

    [HttpGet]
    public async Task<IActionResult> GetFlashcardsBySetId(int flashcardSetId)
    {
        var flashcards = await _flashcardService.GetFlashcardsBySetId(flashcardSetId);
        return Ok(flashcards);
    }

    [HttpPatch]
    [Route("{flashcardId}")]
    public async Task<IActionResult> UpdateFlashcardById(int flashcardId, [FromBody] UpdateFlashcardDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        await _flashcardService.UpdateFlashcard(flashcardId, request);
        return Ok();
    }

    [HttpDelete("{flashcardId}")]
    public async Task<IActionResult> DeleteFlashcard(int flashcardId)
    {
        await _flashcardService.DeleteFlashcard(flashcardId);
        return Ok();
    }
}