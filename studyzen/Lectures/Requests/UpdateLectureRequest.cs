namespace StudyZen.Lectures.Requests;

public sealed class UpdateLectureRequest
{
    public string? Name { get; }
    public string? Content { get; }
    public UpdateLectureRequest(string? name, string? content)
    {
        Name = name;
        Content = content;
    }
}