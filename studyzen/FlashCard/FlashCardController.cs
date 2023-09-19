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

        var flashCard = _flashCardService.AddFlashCard(request);
   
        return CreatedAtAction(nameof(GetFlashcard), new { flashCardId = flashCard.Id }, flashCard);
    }

    [HttpGet]
    [Route("{flashCardId}")]
    public IActionResult GetFlashcard(int flashCardId)
    {
        var flashCard = _flashCardService.GetFlashCard(flashCardId);
        
        return flashCard == null ? NotFound() : Ok(flashCard);
    }
    

    [HttpGet]
    public async Task<IActionResult> GetFlashCardsBySetId(int? flashCardSetId)
    {
        return Ok(_flashCardService.GetFlashCardsBySetId(flashCardSetId));
    }
    

    [HttpDelete("{flashCardId}")]
    public IActionResult DeleteFlashcard(int flashCardId)
    {
        var deleted = _flashCardService.DeleteFlashCard(flashCardId);
        
        if (!deleted)
        {
            return NotFound(); 
        }

        return Ok("Flashcard was deleted successfully");
    }


    [HttpPut("{flashCardId}")]
    public IActionResult UpdateFlashcard(int flashCardId, [FromBody] CreateFlashCardRequest? request)
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
