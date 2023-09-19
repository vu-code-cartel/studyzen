using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.FlashCards.Requests;
using Microsoft.AspNetCore.Mvc.ApiExplorer;


namespace StudyZen.FlashCardSets;

[ApiController]
[Route("[controller]")]
public sealed class FlashCardSetController : ControllerBase
{
    private readonly IFlashCardSetService _flashCardSetService;

    public FlashCardSetController(IFlashCardSetService flashCardSetService)
    {
        _flashCardSetService = flashCardSetService;
    }

    
    [HttpPost("add-flashcardset")]
        public IActionResult AddFlashCardSet([FromBody] CreateFlashCardSetRequest? request)
        {
            request = request.ThrowIfRequestArgumentNull(nameof(request));

            var flashCardSetId = _flashCardSetService.AddFlashCardSet(request);

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
        
        var flashCardSet = _flashCardSetService.GetFlashCardSet(flashCardSetId);
       
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

     
    [HttpGet("all-flashcardsets")]
    public IActionResult GetAllFlashCardSets()
    {
        var flashCardSets = _flashCardSetService.GetAllFlashCardSets();

        if (flashCardSets == null || flashCardSets.Count == 0)
        {
            return NoContent(); 
        }

        return Ok(flashCardSets);
    }

    [HttpDelete("delete-flashcardset/{flashCardSetId}")]
    public IActionResult DeleteFlashcardSet(int flashCardSetId)
    {
        var deleted = _flashCardSetService.DeleteFlashCardSet(flashCardSetId);
        if (!deleted)
        {
            return NotFound(); 
        }

        return Ok("Flashcard set was deleted successfully");
    }



}
