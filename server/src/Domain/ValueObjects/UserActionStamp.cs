using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace StudyZen.Domain.ValueObjects;
[Owned]
public record UserActionStamp
{
    [Required]
    public string User { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }


    public UserActionStamp(string user, DateTime timestamp)
    {
        User = user;
        Timestamp = timestamp;
    }
}