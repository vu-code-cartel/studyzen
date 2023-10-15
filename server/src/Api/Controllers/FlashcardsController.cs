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

    public FlashcardsController(IFlashcardService flashcardService)
    {
        _flashcardService = flashcardService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFlashcard([FromBody] CreateFlashcardDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var newFlashcard = await _flashcardService.CreateFlashcard(request);
        return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = newFlashcard.Id }, newFlashcard);
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