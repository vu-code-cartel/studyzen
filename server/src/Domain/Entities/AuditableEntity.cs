using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace StudyZen.Domain.Entities;

public abstract class AuditableEntity : BaseEntity, IAuditable
{
    [Required]
    public UserActionStamp CreatedBy { get; set; } = null!;

    [Required]
    public UserActionStamp UpdatedBy { get; set; } = null!;

    protected AuditableEntity(int id = default) : base(id)
    {
    }
}
