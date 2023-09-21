namespace StudyZen.FlashCardSets.Requests;

public sealed class UpdateFlashCardSetRequest
{
    public int? LectureId { get; }
    public string? Name { get; }
    public Color? Color { get; }

    public UpdateFlashCardSetRequest(string? name, Color? color, int? lectureId)
    {
        Name = name;
        Color = color;
        LectureId = lectureId;
    }
}