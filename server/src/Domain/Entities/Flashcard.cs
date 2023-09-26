namespace StudyZen.Domain.Entities;

public sealed class Flashcard : BaseEntity
{
    public int FlashcardSetId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }

    public Flashcard(int flashcardSetId, string question, string answer) : base(default)
    {
        FlashcardSetId = flashcardSetId;
        Question = question;
        Answer = answer;
    }
}