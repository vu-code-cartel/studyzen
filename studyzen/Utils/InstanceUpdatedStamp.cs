public struct InstanceUpdatedStamp
{
    public string User { get; set; }
    public DateTime Timestamp { get; set; }
    public InstanceUpdatedStamp(string user = "anonymous")
    {
        User = user;
        Timestamp = DateTime.Now;
    }
}