using StudyZen.Common;
using StudyZen.FlashCardSets;

namespace StudyZen.FlashCardSetClass
{
    public class FlashCardSet : BaseEntity
    {
        public int? LectureId { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }

        public FlashCardSet(int? lectureId, string name, Color color) : base(default)
        {
            LectureId = lectureId;
            Name = name;
            Color = color;
        }
    }
}