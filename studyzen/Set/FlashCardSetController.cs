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

    
    [HttpPost]
        public IActionResult AddFlashCardSet([FromBody] CreateFlashCardSetRequest? request)
        {
            request = request.ThrowIfRequestArgumentNull(nameof(request));

            var flashCardSet= _flashCardSetService.AddFlashCardSet(request);

            return CreatedAtAction(nameof(GetFlashCardSet), new { flashCardSetId = flashCardSet.Id }, flashCardSet);
        }

    [HttpGet("{flashCardSetId}")]
    public IActionResult GetFlashCardSet(int flashCardSetId)
    {
        
        var flashCardSet = _flashCardSetService.GetFlashCardSet(flashCardSetId);

        return flashCardSet == null ? NotFound() : Ok(flashCardSet);
       
    }


    [HttpDelete("{flashCardSetId}")]
    public IActionResult DeleteFlashcardSet(int flashCardSetId)
    {
        var deleted = _flashCardSetService.DeleteFlashCardSet(flashCardSetId);
        if (!deleted)
        {
            return NotFound(); 
        }

        return Ok("Flashcard set was deleted successfully");
    }

    [HttpGet]
    public async Task<IActionResult> GetFlashCardSetsByLectureId(int? lectureId)
    {
        return Ok(_flashCardSetService.GetFlashCardSetsByLectureId(lectureId));
    }



}
