using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.FlashCards.Requests;
using Microsoft.AspNetCore.Mvc.ApiExplorer;


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
    
    [HttpPost("add-flashcardset")]
        public IActionResult AddFlashCardSet([FromBody] CreateFlashCardSetRequest? request)
        {
            request = request.ThrowIfRequestArgumentNull(nameof(request));

            var flashCardSetId = _flashcardService.AddFlashCardSet(request);

            var response = new
            {
                FlashCardSetId = flashCardSetId,
                SetName = request.SetName,
                Color = request.Color,
                LectureId = request.LectureId
            };

            return CreatedAtAction(nameof(GetFlashCardSet), new { flashCardSetId = flashCardSetId }, response);
        }

    [HttpGet("get-flashcardset/{flashCardSetId}")]
    public IActionResult GetFlashCardSet(int flashCardSetId)
    {
        
        var flashCardSet = _flashcardService.GetFlashCardSet(flashCardSetId);
       
        if (flashCardSet == null)
        {
            return NotFound();
        }

        var response = new
    {
        FlashCardSetId = flashCardSet.Id,
        Name = flashCardSet.Name,
        Color = flashCardSet.Color,
        LectureId = flashCardSet.LectureId
    };

        return Ok(response);

       
    }

     [HttpGet("all-flashcards")]
        public IActionResult GetAllFlashcards()
        {
            var flashcards = _flashcardService.GetAllFlashCards();

            if (flashcards == null || flashcards.Count == 0)
            {
                return NoContent(); 
            }

            return Ok(flashcards);
        }

        [HttpGet("all-flashcardsets")]
        public IActionResult GetAllFlashCardSets()
        {
            var flashCardSets = _flashcardService.GetAllFlashCardSets();

            if (flashCardSets == null || flashCardSets.Count == 0)
            {
                return NoContent(); 
            }

            return Ok(flashCardSets);
        }

    

    [HttpDelete("delete-flashcard/{flashcardId}")]
    public IActionResult DeleteFlashcard(int flashcardId)
    {
        var deleted = _flashcardService.DeleteFlashCard(flashcardId);
        if (!deleted)
        {
            return NotFound(); 
    }

    return Ok("Flashcard was deleted successfully");
    }

    [HttpDelete("delete-flashcardset/{flashCardSetId}")]
    public IActionResult DeleteFlashcardSet(int flashCardSetId)
    {
        var deleted = _flashcardService.DeleteFlashCardSet(flashCardSetId);
        if (!deleted)
        {
            return NotFound(); 
        }

        return Ok("Flashcard set was deleted successfully");
    }


    [HttpPut("update-flashcard/{flashcardId}")]
    [CopyFlashCardIdFromRoute] 
    public IActionResult UpdateFlashcard(int flashcardId, [FromBody] UpdateFlashCardRequest request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));

        var existingFlashcard = _flashcardService.GetFlashcard(flashcardId);

        if (existingFlashcard == null)
        {   
            return NotFound();
        }

        existingFlashcard.Question = request.Question;
        existingFlashcard.Answer = request.Answer;

        _flashcardService.UpdateFlashCard(existingFlashcard);

        return Ok("Flashcard updated successfully");
    }

    


}
