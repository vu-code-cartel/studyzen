using StudyZen.Domain.Constraints;
using System.ComponentModel.DataAnnotations;
namespace StudyZen.Domain.Entities;

public sealed class Course : AuditableEntity
{
    [Required]
    [StringLength(CourseConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(CourseConstraints.DescriptionMaxLength)]
    public string Description { get; set; }

    public List<Lecture> Lectures { get; set; } = new();

    public Course(string name, string description)
    {
        Name = name;
        Description = description;
    }
}