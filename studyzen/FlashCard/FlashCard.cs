using StudyZen.Common;
namespace StudyZen.FlashCards;

public class FlashCard : BaseEntity
{
  public int SetId { get; set; }
  public string Question { get; set; }

  public string Answer { get; set; }

  public FlashCard(int setId, string question, string answer) : base(default)
  {
    SetId = setId;
    Question = question;
    Answer = answer;
  }
}
