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

        var response = new
        {

            FlashCardId = flashcardId,
            Question = request.Question,
            Answer = request.Answer

        };


        return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = flashcardId }, response);
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

    // [HttpPost]
    // [Route("CreateFlashcardSet")]
    // public IActionResult CreateFlashcardSet([FromBody] CreateFlashCardSetRequest request)
    // {
    //     request = request.ThrowIfRequestArgumentNull(nameof(request));


    //     var flashcards = new List<FlashCard>();
    //     foreach (var flashCardRequest in request.FlashCards)
    //     {
    //         var flashcardId = _flashcardService.AddFlashcard(flashCardRequest);
    //         var flashcard = _flashcardService.GetFlashcard(flashcardId);
    //         if (flashcard != null)
    //         {
    //             flashcards.Add(flashcard);
    //         }
    //     }


    //     var setId = _flashcardService.CreateFlashcardSet(request.SetName, request.Color, request.LectureId);


    //     foreach (var flashcard in flashcards)
    //     {
    //         _flashcardService.AddFlashcardToSet(setId, flashcard.Id);
    //     }

    //     var response = new
    //     {
    //         SetId = setId,
    //         SetName = request.SetName,
    //         Color = request.Color,
    //         FlashCards = flashcards,
    //         LectureId = request.LectureId
    //     };

    //     return CreatedAtAction(nameof(GetFlashcardSet), new { setId = setId }, response);
    // }

    // [HttpGet]
    // [Route("GetFlashcardSet/{setId}")]
    // public IActionResult GetFlashcardSet(int setId)
    // {
    //     var flashcardSet = _flashcardService.GetFlashcardSet(setId);
    //     if (flashcardSet == null)
    //     {
    //         return NotFound();
    //     }

    //     return Ok(flashcardSet);
    // }

    // [HttpDelete]
    // [Route("DeleteFlashcardSet/{setId}")]
    // public IActionResult DeleteFlashcardSet(int setId)
    // {

    //     var deleted = _flashcardService.DeleteFlashcardSet(setId);


    //     if (!deleted)
    //     {
    //         return NotFound();
    //     }

    //     return Ok("Flashcard set was deleted successfully");
    // }

    // [HttpDelete]
    // [Route("DeleteFlashcard/{flashcardId}")]
    // public IActionResult DeleteFlashcard(int flashcardId)
    // {
    //     var deleted = _flashcardService.DeleteFlashCard(flashcardId);
    //     if (!deleted)
    //     {
    //         return NotFound();
    //     }

    //     return Ok("Flashcard was deleted successfully");
    // }





}
