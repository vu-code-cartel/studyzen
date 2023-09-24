namespace StudyZen.Flashcards.Requests;

public sealed class UpdateFlashcardRequest
{
    public int? LectureId { get; }
    public string? Question { get; }
    public string? Answer { get; }

    public UpdateFlashcardRequest(int? lectureId, string? question, string? answer)
    {
        LectureId = lectureId;
        Question = question;
        Answer = answer;
    }
}
