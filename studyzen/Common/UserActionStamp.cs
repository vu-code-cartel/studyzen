namespace StudyZen.Common;

public sealed record UserActionStamp(string User = "anonymous")
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}