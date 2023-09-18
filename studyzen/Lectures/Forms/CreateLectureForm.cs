namespace StudyZen.Lectures.Forms;

public sealed class CreateLectureForm
{
    public string Name { get; }
    public string? Content { get; }
    public IFormFile File { get; set; }

    public CreateLectureForm(string name, string? content, IFormFile file)
    {
        Name = name;
        Content = content;
        File = file;
    }
}