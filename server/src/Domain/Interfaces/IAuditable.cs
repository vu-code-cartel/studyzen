using StudyZen.Domain.ValueObjects;

namespace StudyZen.Domain.Interfaces;

public interface IAuditable
{
    public UserActionStamp CreatedBy { get; set; }
    public UserActionStamp UpdatedBy { get; set; }
}