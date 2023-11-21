using Microsoft.EntityFrameworkCore;
using Moq;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Services;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Persistence;

namespace StudyZen.Infrastructure.UnitTests.Repositories;

[TestFixture]
public class FlashcardSetRepositoryTests
{
    private FlashcardSetRepository _flashcardSetRepository;
    private ApplicationDbContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        var currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
        currentUserAccessorMock.Setup(c => c.GetUserId()).Returns(Guid.NewGuid().ToString());
        _dbContext = new ApplicationDbContext(options, currentUserAccessorMock.Object);

        _flashcardSetRepository = new FlashcardSetRepository(_dbContext);

        AddTestData();
    }

    public void AddTestData()
    {
        var course = new Course("Course1", "Description1");
        _dbContext.Courses.Add(course);

        var lecture = new Lecture(course.Id, "Lecture1", "Content1");
        _dbContext.Lectures.Add(lecture);

        var flashcardSets = new List<FlashcardSet>
        {
            new FlashcardSet(lecture.Id, "Name1", Domain.Enums.Color.Default),
            new FlashcardSet(null, "Name2", Domain.Enums.Color.Default)
        };
        _dbContext.FlashcardSets.AddRange(flashcardSets);

        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task Get_WithoutParameters_ReturnsAllLectures()
    {
        var retrievedFlashcardSets = await _flashcardSetRepository.Get();

        Assert.That(retrievedFlashcardSets.Count, Is.EqualTo(2));

        var retrievedFlashcardSet1 = retrievedFlashcardSets.FirstOrDefault(f => f.Id == 1);
        var retrievedFlashcardSet2 = retrievedFlashcardSets.FirstOrDefault(f => f.Id == 2);

        Assert.IsNotNull(retrievedFlashcardSet1);
        Assert.That(retrievedFlashcardSet1.Name, Is.EqualTo("Name1"));
        Assert.That(retrievedFlashcardSet1.Color, Is.EqualTo(Domain.Enums.Color.Default));

        Assert.IsNotNull(retrievedFlashcardSet2);
        Assert.That(retrievedFlashcardSet2.Name, Is.EqualTo("Name2"));
        Assert.That(retrievedFlashcardSet2.Color, Is.EqualTo(Domain.Enums.Color.Default));
    }

    [Test]
    public async Task Get_WithFiltering_ReturnsFilteredFlashcardSets()
    {
        var retrievedFlashcardSets = await _flashcardSetRepository.Get(l => l.Name == "Name1");

        Assert.That(retrievedFlashcardSets.Count, Is.EqualTo(1));
        Assert.That(retrievedFlashcardSets.First().Name, Is.EqualTo("Name1"));
    }

    [Test]
    public async Task Get_WithOrdering_ReturnsOrderedFlashcardSets()
    {
        var retrievedFlashcardSets = await _flashcardSetRepository.Get(orderBy: q => q.OrderBy(f => f.Name));

        Assert.That(retrievedFlashcardSets.First().Name, Is.EqualTo("Name1"));
        Assert.That(retrievedFlashcardSets.Last().Name, Is.EqualTo("Name2"));
    }

    [Test]
    public async Task Get_WithSkipAndTake_ReturnsPaginatedFlashcardSets()
    {
        var retrievedFlashcardSets = await _flashcardSetRepository.Get(skip: 1, take: 1);

        Assert.That(retrievedFlashcardSets.Count, Is.EqualTo(1));
        Assert.That(retrievedFlashcardSets.First().Name, Is.EqualTo("Name2"));
    }

    [Test]
    public async Task GetFlashcardSets_IncludesFlashcards_FlashcardsAreLoaded()
    {
        var flashcardSet = new FlashcardSet(null, "Name", Domain.Enums.Color.Default);
        var flashcard = new Flashcard(flashcardSet.Id, "Front", "Back");
        flashcardSet.Flashcards.Add(flashcard);
        _dbContext.FlashcardSets.Add(flashcardSet);
        await _dbContext.SaveChangesAsync();

        var retrievedFlashcardSets = await _flashcardSetRepository.Get(f => f.Id == flashcardSet.Id, includes: f => f.Flashcards);

        Assert.That(retrievedFlashcardSets.Count, Is.EqualTo(1));

        var retrievedFlashcardSet = retrievedFlashcardSets.First();

        Assert.IsNotNull(retrievedFlashcardSet.Flashcards);
        Assert.That(retrievedFlashcardSet.Flashcards.Count, Is.EqualTo(1));

        var retrievedFlashcard = retrievedFlashcardSet.Flashcards.First();

        Assert.That(retrievedFlashcard.Id, Is.EqualTo(flashcard.Id));
    }

    [Test]
    public async Task GetFlashcardSet_IncludesLecture_LectureIsLoaded()
    {
        var flashcardSetId = 1;
        var lectureId = 1;
        var flashcardSets = await _flashcardSetRepository.Get(f => f.Id == flashcardSetId, includes: f => f.Lecture);

        Assert.That(flashcardSets.Count, Is.EqualTo(1));

        var retrievedFlashcardSet = flashcardSets.First();

        Assert.IsNotNull(retrievedFlashcardSet.Lecture);
        Assert.That(retrievedFlashcardSet.Lecture.Id, Is.EqualTo(lectureId));
    }

    [Test]
    public void Update_FlashcardSetExists_FlashcardSetUpdated()
    {
        var flashcardSet = new FlashcardSet(null, "OldName", Domain.Enums.Color.Default);
        _dbContext.FlashcardSets.Add(flashcardSet);
        _dbContext.SaveChanges();

        flashcardSet.Name = "NewName";
        flashcardSet.Color = Domain.Enums.Color.Red;
        _flashcardSetRepository.Update(flashcardSet);
        _dbContext.SaveChanges();

        Assert.That(flashcardSet.Name, Is.EqualTo("NewName"));
        Assert.That(flashcardSet.Color, Is.EqualTo(Domain.Enums.Color.Red));
    }

    [Test]
    public async Task GetFlashcardsBySet_ValidFlashcardSetId_ReturnsCorrectFlashcards()
    {
        var flashcardSet = new FlashcardSet(null, "Name", Domain.Enums.Color.Default);
        var flashcard1 = new Flashcard(flashcardSet.Id, "Question1", "Answer1");
        var flashcard2 = new Flashcard(flashcardSet.Id, "Question2", "Answer2");

        flashcardSet.Flashcards = new List<Flashcard> { flashcard1, flashcard2 };
        _dbContext.FlashcardSets.Add(flashcardSet);
        await _dbContext.SaveChangesAsync();

        var retrievedFlashcards = await _flashcardSetRepository.GetFlashcardsBySet(flashcardSet.Id);

        Assert.That(retrievedFlashcards.Count, Is.EqualTo(2));
        CollectionAssert.AreEquivalent(retrievedFlashcards, flashcardSet.Flashcards);
    }

    [Test]
    public void GetFlashcardsBySet_InvalidFlashcardSetId_ThrowsException()
    {
        var flashcardSetId = 0;
        Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _flashcardSetRepository.GetFlashcardsBySet(flashcardSetId)
            );
    }
}