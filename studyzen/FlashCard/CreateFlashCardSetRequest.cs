using StudyZen.FlashCards;
using StudyZen.Common;
using StudyZen.FlashCards.Requests;

public sealed class CreateFlashCardSetRequest
{
    public string SetName { get; }
    public FlashCardSetColor Color { get; }
    public List<CreateFlashCardRequest> FlashCards { get; }

    public CreateFlashCardSetRequest(string setName, FlashCardSetColor color, List<CreateFlashCardRequest> flashCards)
    {
        SetName = setName.ThrowIfRequestArgumentNull(nameof(setName));
        Color = color;
        FlashCards = flashCards;
    }
}
