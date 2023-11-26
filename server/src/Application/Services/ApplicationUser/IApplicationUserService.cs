using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IApplicationUserService
{
    Task CreateApplicationUser(RegisterApplicationUserDto dto);
    Task<Tokens> AuthenticateApplicationUser(LoginApplicationUserDto dto);
    Task<Tokens> RefreshApplicationUserTokens(string token);
    Task<ApplicationUserDto> GetUser();
}
