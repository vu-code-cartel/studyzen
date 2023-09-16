using StudyZen.Common;

namespace StudyZen.Lectures.Requests;

public sealed class CreateLectureRequest
{
    public string Name { get; }
    public string? Content { get; }

    public CreateLectureRequest(string? name, string? content)
    {
        Name = name.ThrowIfRequestArgumentNull(nameof(name));
        Content = content;
    }
}