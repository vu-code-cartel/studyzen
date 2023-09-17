namespace StudyZen.Utils;

public record UserActionStamp(string User = "anonymous")
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}