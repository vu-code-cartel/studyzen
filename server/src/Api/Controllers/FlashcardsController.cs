using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using FluentValidation;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("flashcards")]
public sealed class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;
    private readonly IValidator<CreateFlashcardDto> _createFlashcardValidator;
    private readonly IValidator<UpdateFlashcardDto> _updateFlashcardValidator;

    public FlashcardsController(IFlashcardService flashcardService, IValidator<CreateFlashcardDto> createFlashcardValidator, IValidator<UpdateFlashcardDto> updateFlashcardValidator)
    {
        _flashcardService = flashcardService;
        _createFlashcardValidator = createFlashcardValidator;
        _updateFlashcardValidator = updateFlashcardValidator;
    }

    [HttpPost]
    public IActionResult CreateFlashcard([FromBody] CreateFlashcardDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var validationResult = _createFlashcardValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        else
        {
            var newFlashcard = _flashcardService.CreateFlashcard(request);
            return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = newFlashcard.Id }, newFlashcard);
        }
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
    public IActionResult UpdateFlashcardById(int flashcardId, [FromBody] UpdateFlashcardDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var validationResult = _updateFlashcardValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        else
        {
            try
            {
                var updatedFlashcard = _flashcardService.UpdateFlashcard(flashcardId, request);
                return updatedFlashcard is null ? NotFound() : Ok(updatedFlashcard);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }
        }
    }

    [HttpDelete("{flashcardId}")]
    public IActionResult DeleteFlashcard(int flashcardId)
    {
        _flashcardService.DeleteFlashcard(flashcardId);
        return NoContent();
    }
}