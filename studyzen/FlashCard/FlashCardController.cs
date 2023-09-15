using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.FlashCards.Requests;



namespace StudyZen.FlashCards;

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
    public IActionResult CreateFlashcard([FromBody] CreateFlashCardRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));

        var flashcardId = _flashcardService.AddFlashcard(request);

        return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = flashcardId }, null);
    }

    [HttpGet]
    [Route("{flashcardId}")]
    public IActionResult GetFlashcard(int flashcardId)
    {
        var flashcard = _flashcardService.GetFlashcard(flashcardId);
        if (flashcard == null)
        {
            return NotFound();
        }

        return Ok(flashcard);
    }
   }
