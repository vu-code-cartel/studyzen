using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public interface ITokenManagementService
{
    Task<string> CreateAccessToken(ApplicationUser applicationUser);
    Task<string> CreateRefreshToken(ApplicationUser applicationUser);
    Task<RefreshToken?> GetRefreshToken(string token);
    Task RevokeRefreshToken(RefreshToken refreshToken);
}