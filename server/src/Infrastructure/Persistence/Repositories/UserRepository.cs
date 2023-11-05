using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Application.Authentication;

namespace StudyZen.Infrastructure.Persistence;

public sealed class UserRepository : Repository<User>, IUserRepository
{
    private readonly IPasswordHasher _passwordHasher;

    public UserRepository(ApplicationDbContext dbContext, IPasswordHasher passwordHasher) : base(dbContext)
    {
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> ValidateCredentials(string username, string password)
    {
        var user = await GetByUsername(username);
        return user != null ? _passwordHasher.VerifyPassword(password, user.HashedPassword) : false;
    }

    public void SetRefreshToken(User user, string refreshToken, DateTime refreshTokenExpiryTime)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetByUsername(string username)
    {
        var usersWithGivenUsername = await Get(
        filter: u => u.Username == username,
        orderBy: null,
        skip: 0,
        take: 1,
        disableTracking: true);

        return usersWithGivenUsername.FirstOrDefault();
    }

}
