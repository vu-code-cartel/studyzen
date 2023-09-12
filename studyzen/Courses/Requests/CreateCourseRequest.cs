using Studyzen.Common;

namespace Studyzen.Courses.Requests;

public sealed record CreateCourseRequest
{
    public string Name { get; }
    public string Description { get; }

    public CreateCourseRequest(string? name, string? description)
    {
        Name = name.ThrowIfRequestArgumentNull(nameof(name));
        Description = description.ThrowIfRequestArgumentNull(nameof(description));
    }
}