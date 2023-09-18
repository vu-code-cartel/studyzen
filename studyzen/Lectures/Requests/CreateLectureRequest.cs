using StudyZen.Common;

namespace StudyZen.Lectures.Requests;

public sealed class CreateLectureRequest
{
    public int CourseId { get; }
    public string Name { get; }
    public string Content { get; }

    public CreateLectureRequest(int courseId, string? name, string? content)
    {
        CourseId = courseId;
        Name = name.ThrowIfRequestArgumentNull(nameof(name));
        Content = content.ThrowIfRequestArgumentNull(nameof(content));
    }
}