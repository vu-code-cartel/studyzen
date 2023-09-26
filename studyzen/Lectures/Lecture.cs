using StudyZen.Common;

namespace StudyZen.Lectures;

public sealed class Lecture : BaseEntity
{
    public int CourseId { get; }
    public string Name { get; set; }
    public string Content { get; set; }

    public Lecture(int courseId, string name, string content) : base(default)
    {
        CourseId = courseId;
        Name = name;
        Content = content;
    }
}