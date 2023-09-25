namespace StudyZen.Common;

public record UserActionStamp(string User = "anonymous")
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}