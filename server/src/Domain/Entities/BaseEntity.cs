using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Domain.Entities;

public abstract class BaseEntity : IAuditable
{
    public int Id { get; set; }
    public UserActionStamp CreatedBy { get; set; } = null!;
    public UserActionStamp UpdatedBy { get; set; } = null!;

    protected BaseEntity(int id)
    {
        Id = id;
    }
}