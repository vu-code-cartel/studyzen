using Microsoft.EntityFrameworkCore;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Persistence;

[TestFixture]
public class LectureRepositoryTests
{
    private LectureRepository _lectureRepository;
    private ApplicationDbContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _dbContext = new ApplicationDbContext(options);

        _lectureRepository = new LectureRepository(_dbContext);

        AddTestData();
    }

    public void AddTestData()
    {
        var course = new Course("Course1", "Description1");
        _dbContext.Courses.Add(course);
        _dbContext.Lectures.Add(new Lecture(course.Id, "Lecture1", "Content1"));
        _dbContext.Lectures.Add(new Lecture(course.Id, "Lecture2", "Content2"));
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
        var retrievedLectures = await _lectureRepository.Get();
        var retrievedLecture1 = retrievedLectures.FirstOrDefault(l => l.Id == 1);
        var retrievedLecture2 = retrievedLectures.FirstOrDefault(l => l.Id == 2);

        Assert.IsNotNull(retrievedLecture1);
        Assert.That(retrievedLecture1.Name, Is.EqualTo("Lecture1"));
        Assert.That(retrievedLecture1.Content, Is.EqualTo("Content1"));

        Assert.IsNotNull(retrievedLecture2);
        Assert.That(retrievedLecture2.Name, Is.EqualTo("Lecture2"));
        Assert.That(retrievedLecture2.Content, Is.EqualTo("Content2"));

        Assert.That(retrievedLectures.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task Get_WithFiltering_ReturnsFilteredLectures()
    {
        var retrievedLectures = await _lectureRepository.Get(l => l.Name == "Lecture1");

        Assert.That(retrievedLectures.Count, Is.EqualTo(1));
        Assert.That(retrievedLectures.First().Name, Is.EqualTo("Lecture1"));
    }

    [Test]
    public async Task Get_WithOrdering_ReturnsOrderedLectures()
    {
        var retrievedLectures = await _lectureRepository.Get(orderBy: q => q.OrderBy(l => l.Name));

        Assert.That(retrievedLectures.First().Name, Is.EqualTo("Lecture1"));
        Assert.That(retrievedLectures.Last().Name, Is.EqualTo("Lecture2"));
    }

    [Test]
    public async Task Get_WithSkipAndTake_ReturnsPaginatedLectures()
    {
        var retrievedLectures = await _lectureRepository.Get(skip: 1, take: 1);

        Assert.That(retrievedLectures.Count, Is.EqualTo(1));
        Assert.That(retrievedLectures.First().Name, Is.EqualTo("Lecture2"));
    }

    [Test]
    public async Task GetLecture_IncludesFlashcardSets_FlashcardSetsAreLoaded()
    {
        var courseId = 1;
        var lecture = new Lecture(courseId, "Sample Lecture", "Lecture Content");
        var flashcardSet = new FlashcardSet(lecture.Id, "Name", StudyZen.Domain.Enums.Color.Default);
        lecture.FlashcardSets.Add(flashcardSet);
        _dbContext.Lectures.Add(lecture);
        await _dbContext.SaveChangesAsync();

        var retrievedLectures = await _lectureRepository.Get(l => l.Id == lecture.Id, includes: l => l.FlashcardSets);

        Assert.That(retrievedLectures.Count, Is.EqualTo(1));

        var retrievedLecture = retrievedLectures.First();

        Assert.IsNotNull(retrievedLecture.FlashcardSets);
        Assert.That(retrievedLecture.FlashcardSets.Count, Is.EqualTo(1));

        var retrievedFlashcardSet = retrievedLecture.FlashcardSets.First();

        Assert.That(retrievedFlashcardSet.Id, Is.EqualTo(flashcardSet.Id));
    }

    [Test]
    public async Task GetLecture_IncludesCourse_CourseIsLoaded()
    {
        var courseId = 1;
        var lectureId = 1;
        var lectures = await _lectureRepository.Get(l => l.Id == lectureId, includes: l => l.Course);

        Assert.That(lectures.Count, Is.EqualTo(1));

        var retrievedLecture = lectures.First();

        Assert.IsNotNull(retrievedLecture.Course);
        Assert.That(retrievedLecture.Course.Id, Is.EqualTo(courseId));
    }

    [Test]
    public void Update_LectureExists_LectureUpdated()
    {
        var course = new Course("Course", "Description");
        _dbContext.Courses.Add(course);
        var lecture = new Lecture(course.Id, "OldLecture", "OldContent");
        _dbContext.Lectures.Add(lecture);
        _dbContext.SaveChanges();

        lecture.Name = "NewLecture";
        lecture.Content = "NewContent";
        _lectureRepository.Update(lecture);
        _dbContext.SaveChanges();

        Assert.That(lecture.Name, Is.EqualTo("NewLecture"));
        Assert.That(lecture.Content, Is.EqualTo("NewContent"));
    }
}