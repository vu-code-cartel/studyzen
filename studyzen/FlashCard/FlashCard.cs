using StudyZen.Common;
namespace StudyZen.FlashCards;

public class FlashCard : BaseEntity
{
 
  public string Question { get; set; }

  public string Answer { get; set; }

  public FlashCard(string question, string answer) : base(default)
  {
   
    Question = question;
    Answer = answer;
  }
}
