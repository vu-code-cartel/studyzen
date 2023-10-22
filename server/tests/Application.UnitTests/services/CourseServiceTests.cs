using Moq;
using StudyZen.Application.Services;
using StudyZen.Application.Repositories;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;
using FluentValidation;

namespace StudyZen.tests.Application.UnitTests;

[TestFixture]
public class CourseServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ValidationHandler> _validationHandlerMock;
    private CourseService _courseService;
    private Mock<ICourseRepository> _courseRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseRepositoryMock = new Mock<ICourseRepository>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(It.IsAny<Type>()))
            .Returns(new object());

        _validationHandlerMock = new Mock<ValidationHandler>(serviceProviderMock.Object);
        _validationHandlerMock.Setup(v => v.ValidateAsync(It.IsAny<CreateCourseDto>()))
                              .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.Courses).Returns(_courseRepositoryMock.Object);

        _courseService = new CourseService(_unitOfWorkMock.Object, _validationHandlerMock.Object);
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

        _unitOfWorkMock.Setup(u => u.Courses.GetById(courseId)).ReturnsAsync(course);

        var result = await _courseService.GetCourseById(courseId);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<CourseDto>(result);
    }

    [Test]
    public async Task GetCourseById_CourseDoesNotExist_ReturnsNull()
    {
        int courseId = 1;

        _unitOfWorkMock.Setup(u => u.Courses.GetById(courseId)).ReturnsAsync(default(Course));

        var result = await _courseService.GetCourseById(courseId);

        Assert.IsNull(result);
    }

    [Test]
    public async Task UpdateCourse_CourseExistsAndValidationPasses_ReturnsTrue()
    {
        var courseId = 1;
        var updateCourseDto = new UpdateCourseDto("new_name", "new_description");
        var course = new Course("name", "description");

        _unitOfWorkMock.Setup(u => u.Courses.GetById(courseId)).ReturnsAsync(course);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateCourseDto)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var result = await _courseService.UpdateCourse(courseId, updateCourseDto);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task UpdateCourse_CourseDoesNotExist_ReturnsFalse()
    {
        var courseId = 1;
        var updateCourseDto = new UpdateCourseDto("new_name", "new_description");

        _unitOfWorkMock.Setup(u => u.Courses.GetById(courseId)).ReturnsAsync(default(Course));

        var result = await _courseService.UpdateCourse(courseId, updateCourseDto);

        Assert.IsFalse(result);
    }

    [Test]
    public void UpdateCourse_CourseExistsButValidationFails_ThrowsValidationException()
    {
        var courseId = 1;
        var updateCourseDto = new UpdateCourseDto("new_name", "new_description");
        var course = new Course("name", "description");

        _unitOfWorkMock.Setup(u => u.Courses.GetById(courseId)).ReturnsAsync(course);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateCourseDto))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _courseService.UpdateCourse(courseId, updateCourseDto));
    }

    [Test]
    public async Task DeleteCourse_CourseExists_ReturnsTrue()
    {
        int courseId = 1;

        _unitOfWorkMock.Setup(u => u.Courses.Delete(courseId)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var result = await _courseService.DeleteCourse(courseId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeleteCourse_CourseDoesNotExist_ReturnsFalse()
    {
        int courseId = 1;

        _unitOfWorkMock.Setup(u => u.Courses.Delete(courseId)).ReturnsAsync(false);

        var result = await _courseService.DeleteCourse(courseId);

        Assert.IsFalse(result);
    }
}

