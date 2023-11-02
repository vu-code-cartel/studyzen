using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace StudyZen.Domain.ValueObjects;

[Owned]
public sealed record UserActionStamp(string User, DateTime Timestamp)
{
    [Required]
    public string User { get; set; } = User;

    [Required]
    public DateTime Timestamp { get; set; } = Timestamp;
}