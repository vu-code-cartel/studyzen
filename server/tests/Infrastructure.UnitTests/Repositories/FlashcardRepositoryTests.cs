using Microsoft.EntityFrameworkCore;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Persistence;

namespace StudyZen.Infrastructure.UnitTests.Repositories;

[TestFixture]
public class FlashcardRepositoryTests
{
    private FlashcardRepository _flashcardRepository;
    private ApplicationDbContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _dbContext = new ApplicationDbContext(options);

        _flashcardRepository = new FlashcardRepository(_dbContext);

        AddTestData();
    }

    void AddTestData()
    {
        var flashcardSet = new FlashcardSet(null, "Name", StudyZen.Domain.Enums.Color.Default);
        _dbContext.FlashcardSets.Add(flashcardSet);

        var flashcards = new List<Flashcard>
        {
            new Flashcard(flashcardSet.Id, "Front1", "Back1"),
            new Flashcard(flashcardSet.Id, "Front2", "Back2")
        };
        _dbContext.Flashcards.AddRange(flashcards);

        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task Get_WithoutParameters_ReturnsAllFlashcards()
    {
        var retrievedFlashcards = await _flashcardRepository.Get();

        Assert.That(retrievedFlashcards.Count, Is.EqualTo(2));

        var retrievedFlashcard1 = retrievedFlashcards.FirstOrDefault(f => f.Id == 1);
        var retrievedFlashcard2 = retrievedFlashcards.FirstOrDefault(f => f.Id == 2);

        Assert.IsNotNull(retrievedFlashcard1);
        Assert.That(retrievedFlashcard1.Front, Is.EqualTo("Front1"));
        Assert.That(retrievedFlashcard1.Back, Is.EqualTo("Back1"));

        Assert.IsNotNull(retrievedFlashcard2);
        Assert.That(retrievedFlashcard2.Front, Is.EqualTo("Front2"));
        Assert.That(retrievedFlashcard2.Back, Is.EqualTo("Back2"));
    }

    [Test]
    public async Task Get_WithFilter_ReturnsFilteredFlashcards()
    {
        var retrievedFlashcards = await _flashcardRepository.Get(f => f.Front == "Front1");

        Assert.That(retrievedFlashcards.Count, Is.EqualTo(1));
        Assert.That(retrievedFlashcards.First().Front, Is.EqualTo("Front1"));
        Assert.That(retrievedFlashcards.First().Back, Is.EqualTo("Back1"));
    }

    [Test]
    public async Task Get_WithOrdering_ReturnsOrderedFlashcards()
    {
        var retrievedFlashcards = await _flashcardRepository.Get(orderBy: q => q.OrderBy(f => f.Front));

        Assert.That(retrievedFlashcards.First().Front, Is.EqualTo("Front1"));
        Assert.That(retrievedFlashcards.First().Back, Is.EqualTo("Back1"));

        Assert.That(retrievedFlashcards.Last().Front, Is.EqualTo("Front2"));
        Assert.That(retrievedFlashcards.Last().Back, Is.EqualTo("Back2"));
    }

    [Test]
    public async Task Get_WithSkipAndTake_ReturnsPaginatedFlashcards()
    {
        var retrievedFlashcards = await _flashcardRepository.Get(skip: 1, take: 1);

        Assert.That(retrievedFlashcards.Count, Is.EqualTo(1));
        Assert.That(retrievedFlashcards.First().Front, Is.EqualTo("Front2"));
        Assert.That(retrievedFlashcards.First().Back, Is.EqualTo("Back2"));
    }
    [Test]
    public async Task GetFlashcard_IncludesFlashcardSet_FlashcardSetIsLoaded()
    {
        var flashcardId = 1;

        var flashcards = await _flashcardRepository.Get(f => f.Id == flashcardId, includes: f => f.FlashcardSet);

        Assert.That(flashcards.Count, Is.EqualTo(1));

        var retrievedFlashcard = flashcards.First();

        Assert.IsNotNull(retrievedFlashcard.FlashcardSet);
        Assert.That(retrievedFlashcard.FlashcardSet.Id, Is.EqualTo(1));
    }

    [Test]
    public void Update_FlashcardExists_FlashcardUpdated()
    {
        var flashcardSet = new FlashcardSet(null, "Name", StudyZen.Domain.Enums.Color.Default);
        _dbContext.FlashcardSets.Add(flashcardSet);
        _dbContext.SaveChanges();

        var flashcard = new Flashcard(flashcardSet.Id, "OldFront", "OldBack");
        _dbContext.Flashcards.Add(flashcard);
        _dbContext.SaveChanges();

        flashcard.Front = "NewFront";
        flashcard.Back = "NewBack";
        _flashcardRepository.Update(flashcard);
        _dbContext.SaveChanges();

        Assert.That(flashcard.Front, Is.EqualTo("NewFront"));
        Assert.That(flashcard.Back, Is.EqualTo("NewBack"));
    }
}