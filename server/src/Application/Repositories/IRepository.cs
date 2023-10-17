using System.Linq.Expressions;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    void Add(params TEntity[] instances);
    Task<TEntity?> GetById(int instanceId, params Expression<Func<TEntity, object>>[] including);
    Task<List<TEntity>> GetAll();
    void Update(TEntity instance);
    void Delete(TEntity instance);
}