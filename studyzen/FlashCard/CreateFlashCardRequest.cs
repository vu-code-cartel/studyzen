using StudyZen.Common;

namespace StudyZen.FlashCards.Requests;

public sealed class CreateFlashCardRequest
{
    public int SetId { get; }
    public string Question { get; }
    public string Answer { get; }

    public CreateFlashCardRequest(int setId, string? question, string? answer)
    {
        SetId = setId;
        Question = question.ThrowIfRequestArgumentNull(nameof(question));
        Answer = answer.ThrowIfRequestArgumentNull(nameof(answer));
    }
}
