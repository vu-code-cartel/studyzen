using System.Linq.Expressions;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    void Add(TEntity instance);
    void AddRange(IEnumerable<TEntity> instances);

    Task<TEntity?> GetById(
        int instanceId,
        params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> GetByIdChecked(
        int instanceId,
        params Expression<Func<TEntity, object>>[] includes);

    Task<List<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int skip = 0,
        int take = int.MaxValue,
        bool disableTracking = true,
        params Expression<Func<TEntity, object>>[] includes);

    void Update(TEntity instance);
    void Delete(TEntity instance);
    Task DeleteByIdChecked(int instanceId);
}