using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Domain.Entities;

public abstract class BaseEntity : IAuditable
{
    public int Id { get; set; }
    public UserActionStamp? CreatedBy { get; set; }
    public UserActionStamp? UpdatedBy { get; set; }

    protected BaseEntity(int id)
    {
        Id = id;
    }
}