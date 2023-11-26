using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using StudyZen.Domain.Constraints;

namespace StudyZen.Domain.Entities;

[ExcludeFromCodeCoverage]
public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(ApplicationUserConstraints.FirstNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(ApplicationUserConstraints.LastNameMaxLength)]
    public string LastName { get; set; } = null!;
}