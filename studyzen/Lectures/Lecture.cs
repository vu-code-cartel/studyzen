using StudyZen.Common;
using StudyZen.Utils;

namespace Studyzen.Lectures;

public sealed class Lecture : BaseEntity
{
    public int CourseId { get; }
    public string Name { get; set; }
    public string? Content { get; set; }
    public InstanceCreatedStamp CreatedBy { get; init; }
    public InstanceUpdatedStamp UpdatedBy { get; set; }

    public List<BaseEntity>? FlashCardSets;

    public Lecture(int courseId, string name, string? content) : base(default)
    {
        CourseId = courseId;
        Name = name;
        Content = content;
        CreatedBy = new InstanceCreatedStamp();
        UpdatedBy = new InstanceUpdatedStamp();
    }
}