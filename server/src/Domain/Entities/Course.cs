using StudyZen.Domain.Constraints;
using System.ComponentModel.DataAnnotations;
namespace StudyZen.Domain.Entities;

public sealed class Course : BaseEntity
{
    [Required]
    [StringLength(CourseConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(CourseConstraints.DescriptionMaxLength)]
    public string Description { get; set; }

    public List<Lecture> Lectures { get; set; } = new();

    public Course(string name, string description) : base(default)
    {
        Name = name;
        Description = description;
    }
}