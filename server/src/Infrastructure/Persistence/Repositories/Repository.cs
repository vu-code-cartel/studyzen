using Serilog;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure.Persistence;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    // TODO: change these to save changes interceptors after lab 2
    // https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
    public Action<TEntity> OnInstanceAdded = delegate { };
    public Action<TEntity> OnInstanceUpdated = delegate { };

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

        OnInstanceAdded += AuditableEntityInterceptor.SetCreateStamp;
        OnInstanceUpdated += AuditableEntityInterceptor.SetUpdateStamp;
    }

    public void Add(TEntity instance)
    {
        var entitySet = GetEntitySet();

        instance.Id = ++entitySet.TotalCount;
        OnInstanceAdded(instance);

        Log.Information("Adding {0} with id {1}", typeof(TEntity).Name, instance.Id);
        entitySet.Instances.Add(instance);

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

        OnInstanceUpdated(instance);

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