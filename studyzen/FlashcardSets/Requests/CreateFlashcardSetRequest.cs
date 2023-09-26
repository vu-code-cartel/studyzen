using StudyZen.Common;

namespace StudyZen.FlashcardSets.Requests;

public sealed class CreateFlashcardSetRequest
{
    public int? LectureId { get; }
    public string Name { get; }
    public Color Color { get; }

    public CreateFlashcardSetRequest(int? lectureId, string? name, Color color)
    {
        LectureId = lectureId;
        Name = name.ThrowIfRequestArgumentNull(nameof(Name));
        Color = color;
    }
}