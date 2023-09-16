namespace StudyZen.Utils;

public record InstanceCreatedStamp
{
    public string User { get; init; }
    public DateTime Timestamp { get; init; }
    public InstanceCreatedStamp(string user = "anonymous")
    {
        User = user;
        Timestamp = DateTime.Now;
    }
}