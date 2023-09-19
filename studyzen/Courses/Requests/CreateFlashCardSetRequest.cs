using StudyZen.FlashCards;
using StudyZen.Common;
using StudyZen.FlashCards.Requests;
using StudyZen.FlashCardSets;



public sealed class CreateFlashCardSetRequest
{
    public string SetName { get; }
    public int SetId { get; private set; }
    public FlashCardSetColor Color { get; }
    
    public int? LectureId {get; }

    public CreateFlashCardSetRequest(string? setName, FlashCardSetColor color, int? lectureId)
    {
       
        SetName = setName.ThrowIfRequestArgumentNull(nameof(setName));
        Color = color;
        LectureId = lectureId;
    }
}
