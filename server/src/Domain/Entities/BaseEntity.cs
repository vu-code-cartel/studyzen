using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyZen.Domain.Interfaces;
using StudyZen.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace StudyZen.Domain.Entities;

public abstract class BaseEntity : IAuditable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Owned]
    public UserActionStamp CreatedBy { get; set; } = null!;

    [Owned]
    public UserActionStamp UpdatedBy { get; set; } = null!;

    protected BaseEntity(int id)
    {
        Id = id;
    }
}