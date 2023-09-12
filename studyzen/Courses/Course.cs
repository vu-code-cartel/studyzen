namespace Studyzen.Courses;

public sealed class Course
{
    public int Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Course(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}