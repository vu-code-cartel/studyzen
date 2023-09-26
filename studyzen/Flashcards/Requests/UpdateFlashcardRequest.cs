namespace StudyZen.Flashcards.Requests;

public sealed class UpdateFlashcardRequest
{
    public string? Question { get; }
    public string? Answer { get; }

    public UpdateFlashcardRequest(string? question, string? answer)
    {
        Question = question;
        Answer = answer;
    }
}