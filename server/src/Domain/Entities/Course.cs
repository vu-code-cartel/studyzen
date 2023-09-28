namespace StudyZen.Domain.Entities;

public sealed class Course : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Course(string name, string description) : base(default)
    {
        Name = name;
        Description = description;
    }
}