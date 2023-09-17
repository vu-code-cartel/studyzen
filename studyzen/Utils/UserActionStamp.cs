namespace StudyZen.Utils;

public record UserActionStamp
{
    public string User { get; init; }
    public DateTime Timestamp { get; init; }

    public UserActionStamp(string user = "anonymous")
    {
        User = user;
        Timestamp = DateTime.Now;
    }
}