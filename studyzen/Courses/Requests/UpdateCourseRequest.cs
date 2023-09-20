using StudyZen.Common;

namespace StudyZen.Courses.Requests;

public sealed class UpdateCourseRequest
{
    public string? Name { get; }
    public string? Description { get; }

    public UpdateCourseRequest(string? name, string? description)
    {
        Name = name;
        Description = description;
    }
}