using StudyZen.Common;
using StudyZen.FlashCards;
namespace StudyZen.FlashCardSetClass
{
   public class FlashCardSet : BaseEntity
  {
      public string Name { get; set; }
      public FlashCardSetColor Color { get; private set; }
   
      public int? LectureId { get; set; }
   
      public FlashCardSet(string name, FlashCardSetColor color, int? lectureId) : base(default)
      {
      
         Name = name;
         Color = color;
         LectureId = lectureId;
      
      }
  }
}
