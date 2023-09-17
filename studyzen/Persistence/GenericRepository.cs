using StudyZen.Common;
using StudyZen.Common.Exceptions;

namespace StudyZen.Persistence;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    void Add(TEntity instance);
    TEntity? GetById(int instanceId);
    List<TEntity> GetAll();
    void Update(TEntity instance);
    void Delete(int instanceId);
}

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly string _filePath;

    public GenericRepository(string fileName)
    {
        _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "StudyZen",
            $"{fileName}.json");
    }

    public void Add(TEntity instance)
    {
        var entitySet = GetEntitySet();

        instance.Id = ++entitySet.TotalCount;
        entitySet.Instances.Add(instance);

        Utilities.WriteToJsonFile(_filePath, entitySet);
    }

    public TEntity? GetById(int instanceId)
    {
        return GetAll().FirstOrDefault(i => i.Id == instanceId);
    }

    public List<TEntity> GetAll()
    {
        return GetEntitySet().Instances;
    }

    public void Update(TEntity instance)
    {
        var entitySet = GetEntitySet();

        var instanceIdx = entitySet.Instances.FindIndex(i => i.Id == instance.Id);
        if (instanceIdx < 0)
        {
            throw new InstanceNotFoundException(typeof(TEntity).Name, instance.Id);
        }

        instance.UpdateUpdatedBy();

        entitySet.Instances[instanceIdx] = instance;

        Utilities.WriteToJsonFile(_filePath, entitySet);
    }

    public void Delete(int instanceId)
    {
        var entitySet = GetEntitySet();

        var instance = entitySet.Instances.FirstOrDefault(i => i.Id == instanceId);
        if (instance is null)
        {
            return;
        }

        entitySet.Instances.Remove(instance);

        Utilities.WriteToJsonFile(_filePath, entitySet);
    }

    private EntitySet<TEntity> GetEntitySet()
    {
        EnsureFileCreated();

        var entitySet = Utilities.ReadFromJsonFile<EntitySet<TEntity>>(_filePath) ?? new EntitySet<TEntity>();

        return entitySet;
    }

    private void EnsureFileCreated()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        if (!File.Exists(_filePath))
        {
            File.Create(_filePath).Close();
        }
    }
}