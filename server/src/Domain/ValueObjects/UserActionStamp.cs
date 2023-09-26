namespace StudyZen.Domain.ValueObjects;

public record struct UserActionStamp(string User)
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public UserActionStamp() : this("anonymous")
    {
    }
}