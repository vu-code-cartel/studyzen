using StudyZen.FlashCards;
using StudyZen.Common;
using StudyZen.FlashCards.Requests;

public sealed class CreateFlashCardSetRequest
{
    public string SetName { get; }
    public int SetId { get; }
    public FlashCardSetColor Color { get; }
    public List<CreateFlashCardRequest> FlashCards { get; }
    public int? LectureId {get; }

    public CreateFlashCardSetRequest(string? setName, FlashCardSetColor color, List<CreateFlashCardRequest> flashCards, int? lectureId)
    {
        //SetId = setId;
        SetName = setName.ThrowIfRequestArgumentNull(nameof(setName));
        Color = color;
        FlashCards = flashCards;
        LectureId = lectureId;
    }
}
