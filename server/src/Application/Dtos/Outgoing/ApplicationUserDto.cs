using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public record ApplicationUserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }

    public ApplicationUserDto(ApplicationUser applicationUser, string role)
    {
        Id = applicationUser.Id;
        Username = applicationUser.UserName!;
        Email = applicationUser.Email!;
        FirstName = applicationUser.FirstName;
        LastName = applicationUser.LastName;
        Role = role;
    }
}