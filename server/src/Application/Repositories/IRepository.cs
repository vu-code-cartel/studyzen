using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task Add(TEntity instance);
    Task<TEntity?> GetById(int instanceId);
    Task<List<TEntity>> GetAll();
    Task<bool> Update(TEntity instance);
    Task<bool> Delete(int instanceId);
}