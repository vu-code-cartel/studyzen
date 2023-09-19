using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.FlashCards.Requests;
using Microsoft.AspNetCore.Mvc.ApiExplorer;


namespace StudyZen.FlashCards;

[ApiController]
[Route("[controller]")]
public sealed class FlashCardsController : ControllerBase
{
    private readonly IFlashCardService _flashCardService;

    public FlashCardsController(IFlashCardService flashCardService)
    {
        _flashCardService = flashCardService;
    }

    [HttpPost]
    public IActionResult CreateFlashcard([FromBody] CreateFlashCardRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));

        var flashCardId = _flashCardService.AddFlashCard(request);

        var response = new
        {
            FlashCardSetId = request.FlashCardSetId,
            FlashCardId = flashCardId,
            Question = request.Question,
            Answer = request.Answer

        };

        return CreatedAtAction(nameof(GetFlashcard), new { flashCardId = flashCardId }, response);
    }

    [HttpGet]
    [Route("{flashCardId}")]
    public IActionResult GetFlashcard(int flashCardId)
    {
        var flashCard = _flashCardService.GetFlashCard(flashCardId);
        if (flashCard == null)
        {
            return NotFound();
        }

        return Ok(flashCard);
    }
    

    [HttpGet("all-flashcards")]
    public IActionResult GetAllFlashcards()
    {
        var flashCards = _flashCardService.GetAllFlashCards();

        if (flashCards == null || flashCards.Count == 0)
        {
            return NoContent(); 
        }

            return Ok(flashCards);
    }

    [HttpGet]
    public async Task<IActionResult> GetFlashCardsBySetId(int? flashCardSetId)
    {
        return Ok(_flashCardService.GetFlashCardsBySetId(flashCardSetId));
    }
    

    [HttpDelete("delete-flashcard/{flashCardId}")]
    public IActionResult DeleteFlashcard(int flashCardId)
    {
        var deleted = _flashCardService.DeleteFlashCard(flashCardId);
        if (!deleted)
        {
            return NotFound(); 
    }

    return Ok("Flashcard was deleted successfully");
    }


    [HttpPut("update-flashcard/{flashCardId}")]
    [CopyFlashCardIdFromRoute] 
    public IActionResult UpdateFlashcard(int flashCardId, [FromBody] UpdateFlashCardRequest request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));

        var existingFlashCard = _flashCardService.GetFlashCard(flashCardId);

        if (existingFlashCard == null)
        {   
            return NotFound();
        }

        existingFlashCard.FlashCardSetId = request.FlashCardSetId;
        existingFlashCard.Question = request.Question;
        existingFlashCard.Answer = request.Answer;

        _flashCardService.UpdateFlashCard(existingFlashCard);

        return Ok("Flashcard updated successfully");
    }

    


}
