using StudyZen.Domain.ValueObjects;

namespace StudyZen.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public UserActionStamp CreatedBy { get; init; }
    public UserActionStamp UpdatedBy { get; set; }

    protected BaseEntity(int id)
    {
        Id = id;
        CreatedBy = new UserActionStamp();
        UpdatedBy = new UserActionStamp();
    }

    public void Update()
    {
        UpdatedBy = new UserActionStamp();
    }
}