using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using FluentValidation;


namespace StudyZen.Infrastructure.Persistence;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    // TODO: change these to save changes interceptors after lab 2
    // https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
    public Action<TEntity> OnInstanceAdded = delegate { };
    public Action<TEntity> OnInstanceUpdated = delegate { };

    protected readonly ApplicationDbContext _dbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        OnInstanceAdded += AuditableEntityInterceptor.SetCreateStamp;
        OnInstanceUpdated += AuditableEntityInterceptor.SetUpdateStamp;
    }

    public async Task Add(TEntity instance)
    {
        OnInstanceAdded(instance);

        _dbContext.Set<TEntity>().Add(instance);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> GetById(int instanceId)
    {
        var instance = await _dbContext.Set<TEntity>().FindAsync(instanceId);
        return instance;
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async void Update(TEntity instance)
    {
        OnInstanceUpdated(instance);

        _dbContext.Update(instance);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> Delete(int instanceId)
    {
        var instance = await _dbContext.Set<TEntity>().FindAsync(instanceId);
        if (instance is null)
        {
            return false;
        }
        _dbContext.Set<TEntity>().Remove(instance);
        await _dbContext.SaveChangesAsync();
        return true;
    }

}