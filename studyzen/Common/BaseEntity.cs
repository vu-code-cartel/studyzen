using StudyZen.Utils;

namespace StudyZen.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public UserActionStamp CreatedBy { get; init; }
    public UserActionStamp UpdatedBy { get; private set; }

    protected BaseEntity(int id)
    {
        Id = id;
        CreatedBy = new UserActionStamp();
        UpdatedBy = new UserActionStamp();
    }

    public void UpdateUpdatedBy()
    {
        UpdatedBy = new UserActionStamp();
    }
}
