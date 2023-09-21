using StudyZen.Common;
using StudyZen.FlashCardSets;


public sealed class CreateFlashCardSetRequest
{
    public int? LectureId { get; }
    public string Name { get; }
    public Color Color { get; }

    public CreateFlashCardSetRequest(int? lectureId, string? name, Color color)
    {
        LectureId = lectureId;
        Name = name.ThrowIfRequestArgumentNull(nameof(Name));
        Color = color;
    }
}