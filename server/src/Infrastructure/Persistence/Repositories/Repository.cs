using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbSet = dbContext.Set<TEntity>();
    }

    public void Add(params TEntity[] instances)
    {
        DbSet.AddRange(instances);
    }

    public async Task<TEntity?> GetById(
        int instanceId,
        params Expression<Func<TEntity, object>>[] including)
    {
        var queryable = DbSet.AsQueryable();

        including
            .ToList()
            .ForEach(include => queryable = queryable.Include(include));

        return await queryable
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == instanceId);
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public void Update(TEntity instance)
    {
        DbSet.Update(instance);
    }

    public void Delete(TEntity instance)
    {
        DbSet.Remove(instance);
    }
}