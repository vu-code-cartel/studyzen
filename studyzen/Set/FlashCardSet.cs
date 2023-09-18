using StudyZen.Common;
using StudyZen.FlashCards;
namespace StudyZen.FlashCardSetClass
{
  public class FlashCardSet : BaseEntity
{
    public string Name { get; set; }
   
    FlashCardSetColor Color { get; set; }
    public List<FlashCard> FlashCards { get; set; } 
    int? LectureId { get; set; }
    public List<int> FlashCardIds { get; } = new List<int>(); 

    public FlashCardSet(string name, FlashCardSetColor color, int? lectureId) : base(default)
    {
      
      Name = name;
      Color = color;
      LectureId = lectureId;
      FlashCards = new List<FlashCard>();
      
    }
}
}
