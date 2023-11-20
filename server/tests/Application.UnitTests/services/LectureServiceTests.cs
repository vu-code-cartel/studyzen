using Moq;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Services;
using StudyZen.Application.Repositories;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;
using FluentValidation;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Application.UnitTests.Services;

[TestFixture]
public class LectureServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ValidationHandler> _validationHandlerMock;
    private LectureService _lectureService;
    private Mock<ICourseRepository> _courseRepositoryMock;
    private Mock<ILectureRepository> _lectureRepositoryMock;
    private Mock<ICurrentUserAccessor> _currentUserAccessorMock;
    private string _userId = Guid.NewGuid().ToString();

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _lectureRepositoryMock = new Mock<ILectureRepository>();
        _courseRepositoryMock = new Mock<ICourseRepository>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(It.IsAny<Type>()))
            .Returns(new object());

        _validationHandlerMock = new Mock<ValidationHandler>(serviceProviderMock.Object);

        _unitOfWorkMock.Setup(u => u.Lectures).Returns(_lectureRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Courses).Returns(_courseRepositoryMock.Object);
        _currentUserAccessorMock = new Mock<ICurrentUserAccessor>(); 
        _currentUserAccessorMock.Setup(c => c.GetUserId()).Returns(_userId);

        _lectureService = new LectureService(_unitOfWorkMock.Object, _validationHandlerMock.Object, _currentUserAccessorMock.Object);
    }

    [Test]
    public async Task CreateLecture_GivenValidDto_ReturnsLectureDto()
    {
        var course = new Course("course_name", "course_desc")
        {
            Id = 1,
            CreatedBy = new UserActionStamp(_userId, DateTime.UtcNow)
        };
        var createLectureDto = new CreateLectureDto(1, "name", "content");

        _unitOfWorkMock.Setup(u => u.Courses.GetByIdChecked(course.Id)).ReturnsAsync(course);
        _unitOfWorkMock.Setup(u => u.Lectures.Add(It.IsAny<Lecture>()));
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var result = await _lectureService.CreateLecture(createLectureDto);

        Assert.NotNull(result);
        Assert.IsInstanceOf<LectureDto>(result);
        _unitOfWorkMock.Verify(u => u.Lectures.Add(It.IsAny<Lecture>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        _currentUserAccessorMock.Verify(c => c.GetUserId(), Times.Once);
    }

    [Test]
    public void CreateLecture_ValidationFailure_ThrowsException()
    {
        var createLectureDto = new CreateLectureDto(1, "failName", "failContent");

        _validationHandlerMock.Setup(v => v.ValidateAsync(It.IsAny<CreateLectureDto>()))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _lectureService.CreateLecture(createLectureDto));
    }

    [Test]
    public async Task GetLectureById_LectureExists_ReturnsLectureDto()
    {
        int lectureId = 1;
        var lecture = new Lecture(1, "name", "content");

        _unitOfWorkMock.Setup(u => u.Lectures.GetByIdChecked(lectureId)).ReturnsAsync(lecture);

        var result = await _lectureService.GetLectureById(lectureId);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<LectureDto>(result);
    }

    [Test]
    public void GetLectureById_LectureDoesNotExist_ThrowsInstanceNotFoundException()
    {
        int lectureId = 1;

        _unitOfWorkMock.Setup(u => u.Lectures.GetByIdChecked(lectureId))
                       .ThrowsAsync(new InstanceNotFoundException("Lecture", lectureId));

        InstanceNotFoundException exception = Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _lectureService.GetLectureById(lectureId));

        Assert.That(exception.Message, Is.EqualTo($"Could not find an instance of 'Lecture' by id {lectureId}"));
    }

    [Test]
    public async Task UpdateLecture_LectureExistsAndValidationPasses_UpdatesLectureDetails()
    {
        var lectureId = 1;
        var updateLectureDto = new UpdateLectureDto("new_name", "new_content");
        var lecture = new Lecture(1, "name", "content")
        {
            CreatedBy = new UserActionStamp(_userId, DateTime.UtcNow)
        };

        _unitOfWorkMock.Setup(u => u.Lectures.GetByIdChecked(lectureId)).ReturnsAsync(lecture);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateLectureDto)).Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        await _lectureService.UpdateLecture(lectureId, updateLectureDto);

        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        _currentUserAccessorMock.Verify(c => c.GetUserId(), Times.Once);

        Assert.That(lecture.Name, Is.EqualTo("new_name"));
        Assert.That(lecture.Content, Is.EqualTo("new_content"));
    }

    [Test]
    public void UpdateLecture_LectureDoesNotExist_ThrowsInstanceNotFoundException()
    {
        var lectureId = 1;
        var updateLectureDto = new UpdateLectureDto("new_name", "new_content");

        _unitOfWorkMock.Setup(u => u.Lectures.GetByIdChecked(lectureId))
                       .Throws(new InstanceNotFoundException("Lecture", lectureId));

        InstanceNotFoundException exception = Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _lectureService.UpdateLecture(lectureId, updateLectureDto)
        );

        Assert.That(exception.Message, Is.EqualTo($"Could not find an instance of 'Lecture' by id {lectureId}"));
    }

    [Test]
    public void UpdateLecture_LectureExistsButValidationFails_ThrowsValidationException()
    {
        var lectureId = 1;
        var updateLectureDto = new UpdateLectureDto("new_name", "new_content");
        var lecture = new Lecture(1, "name", "content")
        {
            CreatedBy = new UserActionStamp(_userId, DateTime.UtcNow)
        };

        _unitOfWorkMock.Setup(u => u.Lectures.GetByIdChecked(lectureId)).ReturnsAsync(lecture);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateLectureDto))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _lectureService.UpdateLecture(lectureId, updateLectureDto));
    }

    [Test]
    public async Task DeleteLecture_LectureExists_MethodsAreCalled()
    {
        int lectureId = 1;
        var lecture = new Lecture(1, "name", "content")
        {
            Id = lectureId,
            CreatedBy = new UserActionStamp(_userId, DateTime.UtcNow)
        };

        _unitOfWorkMock.Setup(u => u.Lectures.GetByIdChecked(lectureId)).Returns(Task.FromResult(lecture));
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        await _lectureService.DeleteLecture(lectureId);

        _unitOfWorkMock.Verify(u => u.Lectures.Delete(lecture), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        _currentUserAccessorMock.Verify(c => c.GetUserId(), Times.Once);
    }

    [Test]
    public void DeleteLecture_LectureDoesNotExist_ThrowsInstanceNotFoundException()
    {
        int lectureId = 1;

        _unitOfWorkMock.Setup(u => u.Lectures.GetByIdChecked(lectureId))
                                   .Throws(new InstanceNotFoundException("Lecture", lectureId));

        Assert.ThrowsAsync<InstanceNotFoundException>(
                async () => await _lectureService.DeleteLecture(lectureId));
    }

    [Test]
    public async Task GetLecturesByCourseId_LecturesExist_ReturnsLecturesDtos()
    {
        int courseId = 1;
        var lectures = new List<Lecture>
    {
        new Lecture(courseId, "name1", "content1"),
        new Lecture(courseId, "name2", "content2")
    };

        _unitOfWorkMock.Setup(u => u.Courses.GetLecturesByCourse(courseId)).ReturnsAsync(lectures);

        var result = await _lectureService.GetLecturesByCourseId(courseId);

        Assert.IsNotEmpty(result);
        Assert.That(result.Count, Is.EqualTo(lectures.Count));


        for (int i = 0; i < lectures.Count; i++)
        {
            var resultFlashcard = result.ElementAt(i);

            Assert.That(lectures[i].Id, Is.EqualTo(resultFlashcard.Id));
            Assert.That(lectures[i].Name, Is.EqualTo(resultFlashcard.Name));
            Assert.That(lectures[i].Content, Is.EqualTo(resultFlashcard.Content));
        }
    }

    [Test]
    public void GetLecturesByCourseId_CourseDoesNotExist_ThrowsInstanceNotFoundException()
    {
        var courseId = 1;

        _unitOfWorkMock.Setup(u => u.Courses.GetLecturesByCourse(courseId))
                                   .Throws(new InstanceNotFoundException("Course", courseId));

        InstanceNotFoundException exception = Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _lectureService.GetLecturesByCourseId(courseId)
        );

        Assert.That(exception.Message, Is.EqualTo($"Could not find an instance of 'Course' by id {courseId}"));
    }
}