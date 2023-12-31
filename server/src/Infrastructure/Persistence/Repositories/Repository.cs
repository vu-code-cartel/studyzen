﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StudyZen.Application.Exceptions;
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

    public void Add(TEntity instance)
    {
        DbSet.Add(instance);
    }

    public void AddRange(IEnumerable<TEntity> instances)
    {
        DbSet.AddRange(instances);
    }

    public async Task<TEntity?> GetById(
        int instanceId,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbSet.AsQueryable();

        includes
            .ToList()
            .ForEach(i => query = query.Include(i));

        return await query.FirstOrDefaultAsync(i => i.Id == instanceId);
    }

    public async Task<TEntity> GetByIdChecked(
        int instanceId,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var instance = await GetById(instanceId, includes);
        return instance ?? throw new InstanceNotFoundException(typeof(TEntity).Name, instanceId);
    }

    public async Task<List<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int skip = 0,
        int take = int.MaxValue,
        bool disableTracking = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = DbSet.AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (skip > 0)
        {
            query = query.Skip(skip);
        }

        if (take >= 0)
        {
            query = query.Take(take);
        }

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        includes
            .ToList()
            .ForEach(i => query = query.Include(i));

        if (orderBy is not null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public void Update(TEntity instance)
    {
        DbSet.Update(instance);
    }

    public void Delete(TEntity instance)
    {
        DbSet.Remove(instance);
    }

    public async Task DeleteByIdChecked(int instanceId)
    {
        var instance = await GetByIdChecked(instanceId);
        Delete(instance);
    }
}