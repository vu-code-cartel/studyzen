namespace StudyZen.Application.Dtos;

public class FlashcardDto 
{
    public int Id { get; set; }
    public int FlashcardSetId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }

}