using Moq;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Services;
using StudyZen.Application.Repositories;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;
using FluentValidation;
using System.Linq.Expressions;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Application.UnitTests.Services;

[TestFixture]
public class CourseServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ValidationHandler> _validationHandlerMock;
    private CourseService _courseService;
    private Mock<ICourseRepository> _courseRepositoryMock;
    private Mock<ICurrentUserAccessor> _currentUserAccessorMock;
    private string _userId = Guid.NewGuid().ToString();

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseRepositoryMock = new Mock<ICourseRepository>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(It.IsAny<Type>()))
            .Returns(new object());

        _validationHandlerMock = new Mock<ValidationHandler>(serviceProviderMock.Object);

        _unitOfWorkMock.SetupGet(u => u.Courses).Returns(_courseRepositoryMock.Object);
        _currentUserAccessorMock = new Mock<ICurrentUserAccessor>();
        _currentUserAccessorMock.Setup(c => c.GetUserId()).Returns(_userId);

        _courseService = new CourseService(
            _unitOfWorkMock.Object, 
            _validationHandlerMock.Object,
            _currentUserAccessorMock.Object);
    }

    [Test]
    public async Task CreateCourse_GivenValidDto_ReturnsCourseDto()
    {
        var createCourseDto = new CreateCourseDto("name", "description");

        _unitOfWorkMock.Setup(u => u.Courses.Add(It.IsAny<Course>()));
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var result = await _courseService.CreateCourse(createCourseDto);

        Assert.NotNull(result);
        Assert.IsInstanceOf<CourseDto>(result);
        _unitOfWorkMock.Verify(u => u.Courses.Add(It.IsAny<Course>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Test]
    public void CreateCourse_ValidationFailure_ThrowsException()
    {
        var createCourseDto = new CreateCourseDto("failName", "failDescription");

        _validationHandlerMock.Setup(v => v.ValidateAsync(It.IsAny<CreateCourseDto>()))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _courseService.CreateCourse(createCourseDto));
    }

    [Test]
    public async Task GetCourseById_CourseExists_ReturnsCourseDto()
    {
        int courseId = 1;
        var course = new Course("name", "description");

        _unitOfWorkMock.Setup(u => u.Courses.GetByIdChecked(courseId)).ReturnsAsync(course);

        var result = await _courseService.GetCourseById(courseId);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<CourseDto>(result);
    }

    [Test]
    public void GetCourseById_CourseDoesNotExist_ThrowsInstanceNotFoundException()
    {
        int courseId = 1;

        _unitOfWorkMock.Setup(u => u.Courses.GetByIdChecked(courseId))
                       .ThrowsAsync(new InstanceNotFoundException("Course", courseId));

        InstanceNotFoundException exception = Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _courseService.GetCourseById(courseId));

        Assert.That(exception.Message, Is.EqualTo($"Could not find an instance of 'Course' by id {courseId}"));
    }

    [Test]
    public async Task UpdateCourse_CourseExistsAndValidationPasses_UpdatesCourseDetails()
    {
        var courseId = 1;
        var updateCourseDto = new UpdateCourseDto("new_name", "new_description");
        var course = new Course("name", "description");
        course.CreatedBy = new UserActionStamp(_userId, DateTime.UtcNow);

        _unitOfWorkMock.Setup(u => u.Courses.GetByIdChecked(courseId)).ReturnsAsync(course);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateCourseDto)).Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        await _courseService.UpdateCourse(courseId, updateCourseDto);

        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        _currentUserAccessorMock.Verify(c => c.GetUserId(), Times.Once);

        Assert.That(course.Name, Is.EqualTo("new_name"));
        Assert.That(course.Description, Is.EqualTo("new_description"));
    }


    [Test]
    public void UpdateCourse_CourseDoesNotExist_ThrowsInstanceNotFoundException()
    {
        var courseId = 1;
        var updateCourseDto = new UpdateCourseDto("new_name", "new_description");

        _unitOfWorkMock.Setup(u => u.Courses.GetByIdChecked(courseId))
                       .Throws(new InstanceNotFoundException("Course", courseId));

        Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _courseService.UpdateCourse(courseId, updateCourseDto)
        );
    }

    [Test]
    public void UpdateCourse_CourseExistsButValidationFails_ThrowsValidationException()
    {
        var courseId = 1;
        var updateCourseDto = new UpdateCourseDto("new_name", "new_description");
        var course = new Course("name", "description");
        course.CreatedBy = new UserActionStamp(_userId, DateTime.UtcNow);

        _unitOfWorkMock.Setup(u => u.Courses.GetById(courseId)).ReturnsAsync(course);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateCourseDto))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _courseService.UpdateCourse(courseId, updateCourseDto));
    }

    [Test]
    public async Task DeleteCourse_CourseExists_MethodsAreCalled()
    {
        int courseId = 1;
        var course = new Course("name", "description")
        {
            CreatedBy = new UserActionStamp(_userId, DateTime.UtcNow)
        };

        _courseRepositoryMock.Setup(c => c.GetByIdChecked(courseId)).Returns(Task.FromResult(course));
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        await _courseService.DeleteCourse(courseId);

        _courseRepositoryMock.Verify(u => u.Delete(course), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        _currentUserAccessorMock.Verify(c => c.GetUserId(), Times.Once);
    }

    [Test]
    public void DeleteCourse_CourseDoesNotExist_ThrowsInstanceNotFoundException()
    {
        int courseId = 1;

        _unitOfWorkMock.Setup(u => u.Courses.GetByIdChecked(courseId))
                                   .Throws(new InstanceNotFoundException("Course", courseId));

        Assert.ThrowsAsync<InstanceNotFoundException>(
                async () => await _courseService.DeleteCourse(courseId));
    }

    [Test]
    public async Task GetAllCourses_ReturnsCourseDtos()
    {
        var courses = new List<Course>
    {
        new Course ("name1", "description1"),
        new Course ("name2", "description2")
    };

        _unitOfWorkMock.Setup(u => u.Courses.Get(null, null, 0, int.MaxValue, true, It.IsAny<Expression<Func<Course, object>>[]>()))
                       .ReturnsAsync(courses);

        var result = await _courseService.GetAllCourses();

        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(courses.Count));
        Assert.IsTrue(result.All(c => c is CourseDto));
    }
}

