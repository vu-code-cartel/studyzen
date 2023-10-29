using Microsoft.EntityFrameworkCore;
using StudyZen.Application.Exceptions;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Persistence;

[TestFixture]
public class RepositoryTests
{
    private class BaseEntityWrapper : BaseEntity
    {
        public BaseEntityWrapper(int id) : base(id)
        {
        }
    }

    private class RepositoryWrapper : Repository<BaseEntityWrapper>
    {
        public RepositoryWrapper(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }

    private class TestApplicationDbContext : ApplicationDbContext
    {
        public DbSet<BaseEntityWrapper> BaseEntityWrappers { get; set; }

        public TestApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }

    private RepositoryWrapper _repository;
    private TestApplicationDbContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;

        _dbContext = new TestApplicationDbContext(options);
        _repository = new RepositoryWrapper(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public void Add_EntityPassed_EntityAddedToDbSet()
    {
        var entityId = 1;

        var entity = new BaseEntityWrapper(entityId);

        _repository.Add(entity);
        _dbContext.SaveChanges();

        var retrievedEntity = _dbContext.Set<BaseEntityWrapper>().Find(entity.Id);
        Assert.IsNotNull(retrievedEntity);
    }

    [Test]
    public void AddRange_MultipleEntitiesPassed_EntitiesAddedToDbSet()
    {
        var entities = new List<BaseEntityWrapper>
    {
        new BaseEntityWrapper(1),
        new BaseEntityWrapper(2),
        new BaseEntityWrapper(3)
    };

        _repository.AddRange(entities);
        _dbContext.SaveChanges();

        foreach (var entity in entities)
        {
            var retrievedEntity = _dbContext.Set<BaseEntityWrapper>().Find(entity.Id);
            Assert.IsNotNull(retrievedEntity);
            Assert.That(retrievedEntity, Is.EqualTo(entity));
        }
    }

    [Test]
    public async Task GetById_ValidId_ReturnsEntity()
    {
        var entityId = 1;

        var entity = new BaseEntityWrapper(entityId);
        _dbContext.Add(entity);
        await _dbContext.SaveChangesAsync();

        var retrievedEntity = await _repository.GetById(entity.Id);

        Assert.IsNotNull(retrievedEntity);
        Assert.That(retrievedEntity.Id, Is.EqualTo(entity.Id));
    }

    [Test]
    public async Task GetById_InvalidId_ReturnsNull()
    {
        var entityId = 1;

        var retrievedEntity = await _repository.GetById(entityId);

        Assert.IsNull(retrievedEntity);
    }

    [Test]
    public async Task GetByIdChecked_EntityExists_ReturnsEntity()
    {
        var entityId = 1;
        var entity = new BaseEntityWrapper(entityId);
        _dbContext.Add(entity);
        await _dbContext.SaveChangesAsync();

        var retrievedEntity = await _repository.GetByIdChecked(entity.Id);

        Assert.IsNotNull(retrievedEntity);
        Assert.That(retrievedEntity.Id, Is.EqualTo(entity.Id));
    }

    [Test]
    public void GetByIdChecked_EntityDoesNotExist_ThrowsInstanceNotFoundException()
    {
        var entityId = 1;
        var ex = Assert.ThrowsAsync<InstanceNotFoundException>(async () => await _repository.GetByIdChecked(entityId));
        Assert.That(ex.Message, Is.EqualTo($"Could not find an instance of 'BaseEntityWrapper' by id {entityId}")); // Checking if the ID appears in the exception message
    }
}