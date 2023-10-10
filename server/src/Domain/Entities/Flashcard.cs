namespace StudyZen.Domain.Entities;

public sealed class Flashcard : BaseEntity
{
    public int FlashcardSetId { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }

    public Flashcard(int flashcardSetId, string front, string back) : base(default)
    {
        FlashcardSetId = flashcardSetId;
        Front = front;
        Back = back;
    }
}