using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyZen.Domain.Constraints;

namespace StudyZen.Domain.Entities;

public sealed class Lecture : BaseEntity
{
    public int CourseId { get; set; }

    [Required]
    [StringLength(LectureConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(LectureConstraints.ContentMaxLength)]
    public string Content { get; set; }

    public Lecture(int courseId, string name, string content) : base(default)
    {
        CourseId = courseId;
        Name = name;
        Content = content;
    }
}