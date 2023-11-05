using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsername(string username);
    Task<bool> ValidateCredentials(string username, string password);
    void SetRefreshToken(User user, string refreshToken, DateTime refreshTokenExpiryTime);
}