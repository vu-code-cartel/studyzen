using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public record ApplicationUserDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ApplicationUserDto(ApplicationUser applicationUser)
    {
        Username = applicationUser.UserName!;
        Email = applicationUser.Email!;
        FirstName = applicationUser.FirstName;
        LastName = applicationUser.LastName;
    }
}