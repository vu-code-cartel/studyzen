using Moq;
using StudyZen.Application.Exceptions;
using StudyZen.Application.Services;
using StudyZen.Application.Repositories;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;
using FluentValidation;
using System.Linq.Expressions;

namespace StudyZen.Application.UnitTests.Services;

[TestFixture]
public class FlashcardSetServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ValidationHandler> _validationHandlerMock;
    private FlashcardSetService _flashcardSetService;
    private Mock<IFlashcardSetRepository> _flashcardSetRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _flashcardSetRepositoryMock = new Mock<IFlashcardSetRepository>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(It.IsAny<Type>()))
            .Returns(new object());

        _validationHandlerMock = new Mock<ValidationHandler>(serviceProviderMock.Object);

        _unitOfWorkMock.Setup(u => u.FlashcardSets).Returns(_flashcardSetRepositoryMock.Object);

        _flashcardSetService = new FlashcardSetService(_unitOfWorkMock.Object, _validationHandlerMock.Object);
    }

    [Test]
    public async Task CreateFlashcardSet_GivenValidDto_ReturnsFlashcardSetDto()
    {
        var createFlashcardSetDto = new CreateFlashcardSetDto(null, "name", Domain.Enums.Color.Default);

        _unitOfWorkMock.Setup(u => u.FlashcardSets.Add(It.IsAny<FlashcardSet>()));
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var result = await _flashcardSetService.CreateFlashcardSet(createFlashcardSetDto);

        Assert.NotNull(result);
        Assert.IsInstanceOf<FlashcardSetDto>(result);
        _unitOfWorkMock.Verify(u => u.FlashcardSets.Add(It.IsAny<FlashcardSet>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Test]
    public void CreateLecture_ValidationFailure_ThrowsException()
    {
        var createFlashcardSetDto = new CreateFlashcardSetDto(null, "failName", Domain.Enums.Color.Default);

        _validationHandlerMock.Setup(v => v.ValidateAsync(It.IsAny<CreateFlashcardSetDto>()))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _flashcardSetService.CreateFlashcardSet(createFlashcardSetDto));
    }

    [Test]
    public async Task GetFlashcardSetById_FlashcardSetExists_ReturnsFlashcardSetDto()
    {
        int flashcardSetId = 1;
        var flashcardSet = new FlashcardSet(null, "name", Domain.Enums.Color.Default);

        _unitOfWorkMock.Setup(u => u.FlashcardSets.GetByIdChecked(flashcardSetId)).ReturnsAsync(flashcardSet);

        var result = await _flashcardSetService.GetFlashcardSetById(flashcardSetId);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<FlashcardSetDto>(result);
    }

    [Test]
    public void GetFlashcardSetById_FlashcardSetDoesNotExist_ThrowsInstanceNotFoundException()
    {
        int flashcardSetId = 1;

        _unitOfWorkMock.Setup(u => u.FlashcardSets.GetByIdChecked(flashcardSetId))
                       .ThrowsAsync(new InstanceNotFoundException("FlashcardSet", flashcardSetId));

        InstanceNotFoundException exception = Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _flashcardSetService.GetFlashcardSetById(flashcardSetId));

        Assert.That(exception.Message, Is.EqualTo($"Could not find an instance of 'FlashcardSet' by id {flashcardSetId}"));
    }

    [Test]
    public async Task UpdateFlashcardSet_FlashcardSetExistsAndValidationPasses_UpdatesFlashcardSetDetails()
    {
        var flashcardSetId = 1;
        var updateFlashcardSetDto = new UpdateFlashcardSetDto("new_name", Domain.Enums.Color.Blue);
        var flashcardSet = new FlashcardSet(null, "name", Domain.Enums.Color.Default);

        _unitOfWorkMock.Setup(u => u.FlashcardSets.GetByIdChecked(flashcardSetId)).ReturnsAsync(flashcardSet);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateFlashcardSetDto)).Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        await _flashcardSetService.UpdateFlashcardSet(flashcardSetId, updateFlashcardSetDto);

        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);

        Assert.That(flashcardSet.Name, Is.EqualTo("new_name"));
        Assert.That(flashcardSet.Color, Is.EqualTo(Domain.Enums.Color.Blue));
    }

    [Test]
    public void UpdateFlashcardSet_FlashcardSetDoesNotExist_ThrowsInstanceNotFoundException()
    {
        var flashcardSetId = 1;
        var updateFlashcardSetDto = new UpdateFlashcardSetDto("new_name", Domain.Enums.Color.Default);

        _unitOfWorkMock.Setup(u => u.FlashcardSets.GetByIdChecked(flashcardSetId))
                       .Throws(new InstanceNotFoundException("FlashcardSet", flashcardSetId));

        InstanceNotFoundException exception = Assert.ThrowsAsync<InstanceNotFoundException>(
            async () => await _flashcardSetService.UpdateFlashcardSet(flashcardSetId, updateFlashcardSetDto)
        );

        Assert.That(exception.Message, Is.EqualTo($"Could not find an instance of 'FlashcardSet' by id {flashcardSetId}"));
    }

    [Test]
    public void UpdateFlashcardSet_FlashcardSetExistsButValidationFails_ThrowsValidationException()
    {
        var flashcardSetId = 1;
        var updateFlashcardSetDto = new UpdateFlashcardSetDto("new_name", Domain.Enums.Color.Blue);
        var flashcardSet = new FlashcardSet(null, "name", Domain.Enums.Color.Default);

        _unitOfWorkMock.Setup(u => u.FlashcardSets.GetById(flashcardSetId)).ReturnsAsync(flashcardSet);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateFlashcardSetDto))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _flashcardSetService.UpdateFlashcardSet(flashcardSetId, updateFlashcardSetDto));
    }

    [Test]
    public async Task DeleteFlashcardSet_FlashcardSetExists_MethodsAreCalled()
    {
        int flashcardSetId = 1;

        _unitOfWorkMock.Setup(u => u.FlashcardSets.DeleteByIdChecked(flashcardSetId)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        await _flashcardSetService.DeleteFlashcardSet(flashcardSetId);

        _unitOfWorkMock.Verify(u => u.FlashcardSets.DeleteByIdChecked(flashcardSetId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Test]
    public void DeleteFlashcardSet_FlashcardSetDoesNotExist_ThrowsInstanceNotFoundException()
    {
        int flashcardSetId = 1;

        _unitOfWorkMock.Setup(u => u.FlashcardSets.DeleteByIdChecked(flashcardSetId))
                                   .Throws(new InstanceNotFoundException("FlashcardSet", flashcardSetId));

        Assert.ThrowsAsync<InstanceNotFoundException>(
                async () => await _flashcardSetService.DeleteFlashcardSet(flashcardSetId));
    }

    [Test]
    public async Task GetFlashcardSets_LectureIdNull_ReturnsFlashcardSetsDtosAsync()
    {
        int? lectureId = null;
        var flashcardSets = new List<FlashcardSet>
    {
        new FlashcardSet(lectureId, "name1", Domain.Enums.Color.Default),
        new FlashcardSet(lectureId, "name2", Domain.Enums.Color.Default)
    };

        _unitOfWorkMock.Setup(u => u.FlashcardSets.Get(
                It.IsAny<Expression<Func<FlashcardSet, bool>>>(),
                It.IsAny<Func<IQueryable<FlashcardSet>, IOrderedQueryable<FlashcardSet>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>(),
                It.IsAny<Expression<Func<FlashcardSet, object>>[]>()
            ))
            .ReturnsAsync(flashcardSets);
        var result = await _flashcardSetService.GetFlashcardSets(null);

        Assert.IsNotEmpty(result);
        Assert.That(result.Count, Is.EqualTo(flashcardSets.Count));


        for (int i = 0; i < flashcardSets.Count; i++)
        {
            var resultFlashcardSet = result.ElementAt(i);

            Assert.That(flashcardSets[i].Id, Is.EqualTo(resultFlashcardSet.Id));
            Assert.That(flashcardSets[i].Name, Is.EqualTo(resultFlashcardSet.Name));
            Assert.That(flashcardSets[i].Color, Is.EqualTo(resultFlashcardSet.Color));
        }
    }
}