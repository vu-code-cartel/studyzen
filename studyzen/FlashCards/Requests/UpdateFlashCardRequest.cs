namespace StudyZen.FlashCards.Requests;

public sealed class UpdateFlashCardRequest
{
    public int? LectureId { get; }
    public string? Question { get; }
    public string? Answer { get; }

    public UpdateFlashCardRequest(int? lectureId, string? question, string? answer)
    {
        LectureId = lectureId;
        Question = question;
        Answer = answer;
    }
}