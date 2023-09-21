using System.Text.Json.Serialization;
using StudyZen.Common;
namespace StudyZen.FlashCards;

public class FlashCard : BaseEntity
{
  public int FlashCardSetId {get; set;}
  public string Question { get; set; }
  public string Answer { get; set; }

  public FlashCard(int flashCardSetId, string question, string answer) : base(default)
  {
    FlashCardSetId = flashCardSetId;
    Question = question;
    Answer = answer;
  }
}
