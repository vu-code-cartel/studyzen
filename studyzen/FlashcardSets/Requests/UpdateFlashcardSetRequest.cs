namespace StudyZen.FlashcardSets.Requests;

public sealed class UpdateFlashcardSetRequest
{
    public string? Name { get; }
    public Color? Color { get; }

    public UpdateFlashcardSetRequest(string? name, Color? color)
    {
        Name = name;
        Color = color;
    }
}