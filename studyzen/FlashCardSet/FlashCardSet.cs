using StudyZen.Common;
using StudyZen.FlashCardSets;
namespace StudyZen.FlashCardSetClass
{
   public class FlashCardSet : BaseEntity
  {
      public string Name { get; set; }
      public Color Color { get; set; }
   
      public int? LectureId { get; set; }
   
      public FlashCardSet(string name, Color color, int? lectureId) : base(default)
      {
      
         Name = name;
         Color = color;
         LectureId = lectureId;
      
      }
  }
}
