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


    [HttpPatch]
    [Route("{flashCardId}")]
    public async Task<IActionResult> UpdateFlashCardById(int flashCardId, [FromBody] UpdateFlashCardRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedFlashCard = _flashCardService.UpdateFlashCardById(flashCardId, request);
        return updatedFlashCard == null ? NotFound() : Ok(updatedFlashCard);
    }

    


}
