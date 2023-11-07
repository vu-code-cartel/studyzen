using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IApplicationUserService
{
    Task<ApplicationUserDto> CreateApplicationUser(RegisterApplicationUserDto dto);
}
