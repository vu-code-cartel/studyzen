using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public sealed class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}