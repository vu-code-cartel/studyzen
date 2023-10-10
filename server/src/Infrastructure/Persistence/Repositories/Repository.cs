using Serilog;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Persistence;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly string _filePath;
    private readonly IFileService _fileService;
    private readonly ApplicationDbContext _dbContext;

    protected Repository(string fileName, IFileService fileService, ApplicationDbContext dbContext)
    {
        _fileService = fileService;
        _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "StudyZen",
            $"{fileName}.json");
        _dbContext = dbContext;
    }

    public void Add(TEntity instance)
    {
        var entitySet = GetEntitySet();

        instance.Id = ++entitySet.TotalCount;
        entitySet.Instances.Add(instance);
        Log.Information("A new instance of {instanceType} with the id {id} was added to {filePath}.", typeof(TEntity).Name, instance.Id, _filePath);

        Log.Information("Adding a new instance of '{0}' with id '{1}'", typeof(TEntity).Name, instance.Id);
        _fileService.WriteToJsonFile(_filePath, entitySet);
    }

    public TEntity? GetById(int instanceId)
    {
        return GetAll().FirstOrDefault(i => i.Id == instanceId);
    }

    public List<TEntity> GetAll()
    {
        return GetEntitySet().Instances;
    }

    public bool Update(TEntity instance)
    {
        var entitySet = GetEntitySet();

        var instanceIdx = entitySet.Instances.FindIndex(i => i.Id == instance.Id);
        if (instanceIdx < 0)
        {
            return false;
        }

        instance.Update();

        entitySet.Instances[instanceIdx] = instance;

        _fileService.WriteToJsonFile(_filePath, entitySet);

        return true;
    }

    public bool Delete(int instanceId)
    {
        var entitySet = GetEntitySet();

        var instance = entitySet.Instances.FirstOrDefault(i => i.Id == instanceId);
        if (instance is null)
        {
            return false;
        }

        entitySet.Instances.Remove(instance);

        _fileService.WriteToJsonFile(_filePath, entitySet);

        return true;
    }

    private EntitySet<TEntity> GetEntitySet()
    {
        EnsureFileCreated();

        var entitySet = _fileService.ReadFromJsonFile<EntitySet<TEntity>>(_filePath) ?? new EntitySet<TEntity>();

        return entitySet;
    }

    private void EnsureFileCreated()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        if (!File.Exists(_filePath))
        {
            File.Create(_filePath).Close();
            Log.Information("File {path} was created", _filePath);
        }
    }
}