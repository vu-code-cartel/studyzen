using System.Text.Json.Serialization;
using StudyZen.Common;

namespace StudyZen.FlashCards.Requests;

public sealed class CreateFlashCardRequest
{
 
    public int FlashCardSetId {get;}
    public string Question { get; }
    public string Answer { get; }

    public CreateFlashCardRequest(int flashCardSetId, string? question, string? answer)
    {
        FlashCardSetId = flashCardSetId;
        Question = question.ThrowIfRequestArgumentNull(nameof(question));
        Answer = answer.ThrowIfRequestArgumentNull(nameof(answer));
    }
}
