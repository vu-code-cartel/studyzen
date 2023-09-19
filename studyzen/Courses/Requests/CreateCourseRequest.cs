using StudyZen.Common;

namespace StudyZen.Courses.Requests;

public sealed class CreateCourseRequest
{
    public string Name { get; }
    public string? Description { get; }

    public CreateCourseRequest(string? name, string? description)
    {
        Name = name.ThrowIfRequestArgumentNull(nameof(name));
        Description = description;
    }
}