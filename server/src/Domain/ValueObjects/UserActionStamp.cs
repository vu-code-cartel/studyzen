using System.ComponentModel.DataAnnotations;

namespace StudyZen.Domain.ValueObjects;

public record UserActionStamp
{
    [Required]
    public string User { get; set; }

    [Required]
    public DateTime TimeStamp { get; set; }


    public UserActionStamp(string user, DateTime timeStamp)
    {
        User = user;
        TimeStamp = timeStamp;
    }
}