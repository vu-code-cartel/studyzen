using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class FlashcardSetsController : ControllerBase
{
    private readonly IFlashcardSetService _flashcardSetService;

    public FlashcardSetsController(IFlashcardSetService flashcardSetService)
    {
        _flashcardSetService = flashcardSetService;
    }

    [HttpPost]
    [Authorize(Roles = "Lecturer, Student")]
    public async Task<IActionResult> CreateFlashcardSet([FromBody] CreateFlashcardSetDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var createdFlashcardSet = await _flashcardSetService.CreateFlashcardSet(request);
        return CreatedAtAction(nameof(GetFlashcardSet), new { flashcardSetId = createdFlashcardSet.Id }, createdFlashcardSet);
    }

    [HttpGet("{flashcardSetId}")]
    public async Task<IActionResult> GetFlashcardSet(int flashcardSetId)
    {
        var flashcardSet = await _flashcardSetService.GetFlashcardSetById(flashcardSetId);
        return Ok(flashcardSet);
    }

    [HttpGet]
    public async Task<IActionResult> GetFlashcardSets(int? lectureId)
    {
        var flashcardSets = await _flashcardSetService.GetFlashcardSets(lectureId);
        return Ok(flashcardSets);
    }

    [HttpPatch("{flashcardSetId}")]
    [Authorize(Roles = "Lecturer, Student")]
    public async Task<IActionResult> UpdateFlashcardSetById(int flashcardSetId, [FromBody] UpdateFlashcardSetDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        await _flashcardSetService.UpdateFlashcardSet(flashcardSetId, request);
        return Ok();
    }

    [HttpDelete("{flashcardSetId}")]
    [Authorize(Roles = "Lecturer, Student")]
    public async Task<IActionResult> DeleteFlashcardSet(int flashcardSetId)
    {
        await _flashcardSetService.DeleteFlashcardSet(flashcardSetId);
        return Ok();
    }
}