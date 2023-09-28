using StudyZen.Domain.Entities;

namespace StudyZen.Application.Repositories;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    void Add(TEntity instance);
    TEntity? GetById(int instanceId);
    List<TEntity> GetAll();
    bool Update(TEntity instance);
    void Delete(int instanceId);
}