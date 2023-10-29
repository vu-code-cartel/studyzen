using Microsoft.EntityFrameworkCore;
using StudyZen.Application.Exceptions;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Persistence;

[TestFixture]
public class CourseRepositoryTests
{
    private CourseRepository _courseRepository;
    private ApplicationDbContext _dbContext;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _dbContext = new ApplicationDbContext(options);

        _courseRepository = new CourseRepository(_dbContext);

    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetLecturesByCourse_CourseExists_ReturnsLectures()
    {
        var course = new Course("Course1", "Description1");
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        var lecture1 = new Lecture(course.Id, "Lecture1", "Content1");
        var lecture2 = new Lecture(course.Id, "Lecture2", "Content2");
        course.Lectures.Add(lecture1);
        course.Lectures.Add(lecture2);
        await _dbContext.SaveChangesAsync();

        var retrievedLectures = await _courseRepository.GetLecturesByCourse(course.Id);

        Assert.IsNotNull(retrievedLectures);
        Assert.That(retrievedLectures.Count, Is.EqualTo(2));

        var retrievedLecture1 = retrievedLectures.FirstOrDefault(l => l.Id == 1);
        var retrievedLecture2 = retrievedLectures.FirstOrDefault(l => l.Id == 2);

        Assert.IsNotNull(retrievedLecture1);
        Assert.That(retrievedLecture1.Name, Is.EqualTo("Lecture1"));
        Assert.That(retrievedLecture1.Content, Is.EqualTo("Content1"));

        Assert.IsNotNull(retrievedLecture2);
        Assert.That(retrievedLecture2.Name, Is.EqualTo("Lecture2"));
        Assert.That(retrievedLecture2.Content, Is.EqualTo("Content2"));
    }

    [Test]
    public void GetLecturesByCourse_CourseDoesNotExist_ThrowsException()
    {
        var courseId = 0;

        Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _courseRepository.GetLecturesByCourse(courseId));
    }

    [Test]
    public async Task Get_WithoutParameters_ReturnsAllCourses()
    {
        _dbContext.Courses.Add(new Course("Course1", "Description1"));
        _dbContext.Courses.Add(new Course("Course2", "Description2"));
        await _dbContext.SaveChangesAsync();

        var retrievedCourses = await _courseRepository.Get();

        Assert.That(retrievedCourses.Count, Is.EqualTo(2));

        var retrievedCourse1 = retrievedCourses.FirstOrDefault(c => c.Id == 1);
        var retrievedCourse2 = retrievedCourses.FirstOrDefault(c => c.Id == 2);

        Assert.IsNotNull(retrievedCourse1);
        Assert.That(retrievedCourse1.Name, Is.EqualTo("Course1"));
        Assert.That(retrievedCourse1.Description, Is.EqualTo("Description1"));

        Assert.IsNotNull(retrievedCourse2);
        Assert.That(retrievedCourse2.Name, Is.EqualTo("Course2"));
        Assert.That(retrievedCourse2.Description, Is.EqualTo("Description2"));
    }

    [Test]
    public async Task Get_WithFilter_ReturnsFilteredCourses()
    {
        _dbContext.Courses.Add(new Course("Course1", "Description1"));
        _dbContext.Courses.Add(new Course("Course2", "Description2"));
        await _dbContext.SaveChangesAsync();

        var retrievedCourses = await _courseRepository.Get(c => c.Name == "Course1");

        Assert.That(retrievedCourses.Count, Is.EqualTo(1));
        Assert.That(retrievedCourses.First().Name, Is.EqualTo("Course1"));
    }

    [Test]
    public async Task Get_WithOrdering_ReturnsOrderedCourses()
    {
        _dbContext.Courses.Add(new Course("Course2", "Description2"));
        _dbContext.Courses.Add(new Course("Course1", "Description1"));
        await _dbContext.SaveChangesAsync();

        var courses = await _courseRepository.Get(orderBy: q => q.OrderBy(c => c.Name));

        Assert.That(courses.First().Name, Is.EqualTo("Course1"));
        Assert.That(courses.Last().Name, Is.EqualTo("Course2"));
    }

    [Test]
    public async Task Get_WithSkipAndTake_ReturnsPaginatedCourses()
    {
        _dbContext.Courses.Add(new Course("Course1", "Description1"));
        _dbContext.Courses.Add(new Course("Course2", "Description2"));
        await _dbContext.SaveChangesAsync();

        var courses = await _courseRepository.Get(skip: 1, take: 1);

        Assert.That(courses.Count, Is.EqualTo(1));
        Assert.That(courses.First().Name, Is.EqualTo("Course2"));
    }

    [Test]
    public async Task Get_WithIncludes_ReturnsCoursesWithRelatedData()
    {
        var course = new Course("Course1", "Description1");
        var lecture = new Lecture(course.Id, "Lecture1", "Content1");
        course.Lectures.Add(lecture);
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        var courses = await _courseRepository.Get(c => c.Id == course.Id, includes: c => c.Lectures);

        Assert.That(courses.Count, Is.EqualTo(1));

        var retrievedCourse = courses.First();
        Assert.That(retrievedCourse.Lectures.Count, Is.EqualTo(1));

        var retrievedLecture = retrievedCourse.Lectures.First();
        Assert.That(retrievedLecture.Name, Is.EqualTo("Lecture1"));
        Assert.That(retrievedLecture.Content, Is.EqualTo("Content1"));
    }

    [Test]
    public void Update_CourseExists_CourseUpdated()
    {
        var course = new Course("OldCourse", "OldDescription");
        _dbContext.Courses.Add(course);
        _dbContext.SaveChanges();

        course.Name = "NewCourse";
        course.Description = "NewDescription";

        _courseRepository.Update(course);
        _dbContext.SaveChanges();

        Assert.That(course.Name, Is.EqualTo("NewCourse"));
        Assert.That(course.Description, Is.EqualTo("NewDescription"));
    }
}