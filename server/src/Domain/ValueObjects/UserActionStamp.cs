using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace StudyZen.Domain.ValueObjects;
[Owned]
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