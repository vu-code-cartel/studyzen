using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using StudyZen.Application.Exceptions;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Persistence;

namespace StudyZen.Infrastructure.UnitTests.Repositories;

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
    int testEntityId;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _dbContext = new TestApplicationDbContext(options);
        _repository = new RepositoryWrapper(_dbContext);

        AddTestData();
    }

    public void AddTestData()
    {
        var entity = new BaseEntityWrapper(testEntityId);

        _repository.Add(entity);
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public void Add_EntityPassed_EntityAddedToDbSet()
    {
        var retrievedEntity = _dbContext.Set<BaseEntityWrapper>().Find(testEntityId);
        Assert.IsNotNull(retrievedEntity);
    }

    [Test]
    public void AddRange_MultipleEntitiesPassed_EntitiesAddedToDbSet()
    {
        var entities = new List<BaseEntityWrapper>
    {
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
        var retrievedEntity = await _repository.GetById(testEntityId);

        Assert.IsNotNull(retrievedEntity);
        Assert.That(retrievedEntity.Id, Is.EqualTo(testEntityId));
    }

    [Test]
    public async Task GetById_InvalidId_ReturnsNull()
    {
        var entityId = 0;

        var retrievedEntity = await _repository.GetById(entityId);

        Assert.IsNull(retrievedEntity);
    }

    [Test]
    public async Task GetByIdChecked_EntityExists_ReturnsEntity()
    {
        var retrievedEntity = await _repository.GetByIdChecked(testEntityId);

        Assert.IsNotNull(retrievedEntity);
        Assert.That(retrievedEntity.Id, Is.EqualTo(testEntityId));
    }

    [Test]
    public void GetByIdChecked_EntityDoesNotExist_ThrowsInstanceNotFoundException()
    {
        var entityId = 0;
        var exception = Assert.ThrowsAsync<InstanceNotFoundException>(async () => await _repository.GetByIdChecked(entityId));
        Assert.That(exception.Message, Is.EqualTo($"Could not find an instance of 'BaseEntityWrapper' by id {entityId}"));
    }

    [Test]
    public void Delete_EntityExists_EntityRemoved()
    {
        var entityId = 2;
        var entity = new BaseEntityWrapper(entityId);
        _dbContext.Add(entity);
        _dbContext.SaveChanges();

        _repository.Delete(entity);
        _dbContext.SaveChanges();

        var deletedEntity = _dbContext.Find<BaseEntityWrapper>(2);
        Assert.IsNull(deletedEntity);
    }

    [Test]
    public async Task DeleteByIdChecked_EntityExists_EntityRemoved()
    {
        await _repository.DeleteByIdChecked(testEntityId);
        await _dbContext.SaveChangesAsync();

        var deletedEntity = await _dbContext.FindAsync<BaseEntityWrapper>(testEntityId);
        Assert.IsNull(deletedEntity);
    }

    [Test]
    public void DeleteByIdChecked_EntityDoesNotExist_ThrowsInstanceNotFoundException()
    {
        var entityId = 0;

        var exception = Assert.ThrowsAsync<InstanceNotFoundException>(async () => await _repository.DeleteByIdChecked(entityId));
        Assert.That(exception.Message, Is.EqualTo($"Could not find an instance of 'BaseEntityWrapper' by id {entityId}"));
    }
}