using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using StudyZen.Domain.Constraints;

namespace StudyZen.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(ApplicationUserConstraints.FirstNameMaxLength)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(ApplicationUserConstraints.LastNameMaxLength)]
    public string LastName { get; set; }

    public ApplicationUser(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}