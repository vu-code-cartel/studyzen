using StudyZen.Common;

namespace StudyZen.Flashcards.Requests;

public sealed class CreateFlashcardRequest
{
    public int FlashcardSetId { get; }
    public string Question { get; }
    public string Answer { get; }

    public CreateFlashcardRequest(int flashcardSetId, string? question, string? answer)
    {
        FlashcardSetId = flashcardSetId;
        Question = question.ThrowIfRequestArgumentNull(nameof(question));
        Answer = answer.ThrowIfRequestArgumentNull(nameof(answer));
    }
}