namespace StudyZen.FlashcardSets.Requests;

public sealed class UpdateFlashcardSetRequest
{
    public int? LectureId { get; }
    public string? Name { get; }
    public Color? Color { get; }

    public UpdateFlashcardSetRequest(string? name, Color? color, int? lectureId)
    {
        Name = name;
        Color = color;
        LectureId = lectureId;
    }
}