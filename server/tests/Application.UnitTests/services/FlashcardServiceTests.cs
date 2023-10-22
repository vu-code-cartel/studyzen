using Moq;
using StudyZen.Application.Services;
using StudyZen.Application.Repositories;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;
using FluentValidation;

namespace StudyZen.tests.Application.UnitTests;

[TestFixture]
public class FlashcardServiceTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ValidationHandler> _validationHandlerMock;
    private FlashcardService _flashcardService;
    private Mock<IFlashcardRepository> _flashcardRepositoryMock;


    [SetUp]
    public void SetUp()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _flashcardRepositoryMock = new Mock<IFlashcardRepository>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(sp => sp.GetService(It.IsAny<Type>()))
            .Returns(new object());

        _validationHandlerMock = new Mock<ValidationHandler>(serviceProviderMock.Object);

        _validationHandlerMock.Setup(v => v.ValidateAsync(It.IsAny<CreateFlashcardDto>()))
                              .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.Flashcards).Returns(_flashcardRepositoryMock.Object);

        _flashcardService = new FlashcardService(_unitOfWorkMock.Object, _validationHandlerMock.Object);
    }

    [Test]
    public async Task CreateFlashcard_GivenValidDto_ReturnsFlashcardDto()
    {
        var createFlashcardDto = new CreateFlashcardDto(1, "front", "back");

        _unitOfWorkMock.Setup(u => u.Flashcards.Add(It.IsAny<Flashcard>()));
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var result = await _flashcardService.CreateFlashcard(createFlashcardDto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf<FlashcardDto>());
        _unitOfWorkMock.Verify(u => u.Flashcards.Add(It.IsAny<Flashcard>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChanges(), Times.Once);
        Assert.That(result.Front, Is.EqualTo(createFlashcardDto.Front));
        Assert.That(result.Back, Is.EqualTo(createFlashcardDto.Back));
    }

    [Test]
    public void CreateFlashcard_ValidationFailure_ThrowsException()
    {
        var createFlashcardDto = new CreateFlashcardDto(0, "", "");

        _validationHandlerMock.Setup(v => v.ValidateAsync(It.IsAny<CreateFlashcardDto>()))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _flashcardService.CreateFlashcard(createFlashcardDto));
    }

    [Test]
    public async Task GetFlashcardById_FlashcardExists_ReturnsFlashcardDto()
    {
        int flashcardId = 1;
        var flashcard = new Flashcard(flashcardId, "front", "back");

        _unitOfWorkMock.Setup(u => u.Flashcards.GetById(flashcardId)).ReturnsAsync(flashcard);

        var result = await _flashcardService.GetFlashcardById(flashcardId);

        Assert.IsNotNull(result);
        Assert.IsInstanceOf<FlashcardDto>(result);
    }

    [Test]
    public async Task GetFlashcardById_FlashcardDoesNotExist_ReturnsNull()
    {
        int flashcardId = 1;

        _unitOfWorkMock.Setup(u => u.Flashcards.GetById(flashcardId)).ReturnsAsync(default(Flashcard));

        var result = await _flashcardService.GetFlashcardById(flashcardId);

        Assert.IsNull(result);
    }

    [Test]
    public async Task UpdateFlashcard_FlashcardExistsAndValidationPasses_ReturnsTrue()
    {
        var flashcardId = 1;
        var updateFlashcardDto = new UpdateFlashcardDto("new_front", "new_back");
        var flashcard = new Flashcard(flashcardId, "front", "back");

        _unitOfWorkMock.Setup(u => u.Flashcards.GetById(flashcardId)).ReturnsAsync(flashcard);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateFlashcardDto)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var result = await _flashcardService.UpdateFlashcard(flashcardId, updateFlashcardDto);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task UpdateFlashcard_FlashcardDoesNotExist_ReturnsFalse()
    {
        var flashcardId = 1;
        var updateFlashcardDto = new UpdateFlashcardDto("new_front", "new_back");

        _unitOfWorkMock.Setup(u => u.Flashcards.GetById(flashcardId)).ReturnsAsync(default(Flashcard));

        var result = await _flashcardService.UpdateFlashcard(flashcardId, updateFlashcardDto);

        Assert.IsFalse(result);
    }

    [Test]
    public void UpdateFlashcard_FlashcardExistsButValidationFails_ThrowsValidationException()
    {
        var flashcardId = 1;
        var updateFlashcardDto = new UpdateFlashcardDto("new_front", "new_back");
        var flashcard = new Flashcard(flashcardId, "front", "back");

        _unitOfWorkMock.Setup(u => u.Flashcards.GetById(flashcardId)).ReturnsAsync(flashcard);
        _validationHandlerMock.Setup(v => v.ValidateAsync(updateFlashcardDto))
                              .ThrowsAsync(new ValidationException("Validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _flashcardService.UpdateFlashcard(flashcardId, updateFlashcardDto));
    }

    [Test]
    public async Task DeleteFlashcard_FlashcardExists_ReturnsTrue()
    {
        int flashcardId = 1;

        _unitOfWorkMock.Setup(u => u.Flashcards.Delete(flashcardId)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.SaveChanges()).ReturnsAsync(1);

        var result = await _flashcardService.DeleteFlashcard(flashcardId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeleteFlashcard_FlashcardDoesNotExist_ReturnsFalse()
    {
        int flashcardId = 1;

        _unitOfWorkMock.Setup(u => u.Flashcards.Delete(flashcardId)).ReturnsAsync(false);

        var result = await _flashcardService.DeleteFlashcard(flashcardId);

        Assert.IsFalse(result);
    }

    [Test]
    public async Task GetFlashcardsBySetId_FlashcardsExist_ReturnsFlashcardDtos()
    {
        int flashcardSetId = 1;
        var flashcards = new List<Flashcard>
    {
        new Flashcard(1, "front1", "back1"),
        new Flashcard(2, "front2", "back2")
    };

        _unitOfWorkMock.Setup(u => u.Flashcards.GetFlashcardsBySetId(flashcardSetId)).ReturnsAsync(flashcards);

        var result = await _flashcardService.GetFlashcardsBySetId(flashcardSetId);

        Assert.IsNotEmpty(result);
        Assert.That(flashcards.Count, Is.EqualTo(result.Count));
    }

    [Test]
    public async Task GetFlashcardsBySetId_NoFlashcardsExist_ReturnsEmptyCollection()
    {
        int flashcardSetId = 1;

        _unitOfWorkMock.Setup(u => u.Flashcards.GetFlashcardsBySetId(flashcardSetId)).ReturnsAsync(new List<Flashcard>());

        var result = await _flashcardService.GetFlashcardsBySetId(flashcardSetId);

        Assert.IsEmpty(result);
    }

    [Test]
    public async Task CreateFlashcards_SuccessfulCreation()
    {
        var dtos = new List<CreateFlashcardDto>
        {
            new CreateFlashcardDto(1, "front1", "back1"),
            new CreateFlashcardDto(1, "front2", "back2")
        };

        _validationHandlerMock.Setup(v => v.ValidateAsync(It.IsAny<CreateFlashcardDto>()))
                              .Returns(Task.CompletedTask);

        _flashcardRepositoryMock.Setup(repo => repo.AddRange(It.IsAny<IEnumerable<Flashcard>>()));

        var result = await _flashcardService.CreateFlashcards(dtos);

        Assert.NotNull(result);
        Assert.That(dtos.Count, Is.EqualTo(result.Count));
    }

    [Test]
    public void CreateFlashcards_ValidationFailure_ThrowsException()
    {
        var dtos = new List<CreateFlashcardDto>
        {
            new CreateFlashcardDto(1, "", "")
        };

        _validationHandlerMock.Setup(v => v.ValidateAsync(It.IsAny<CreateFlashcardDto>()))
                              .ThrowsAsync(new ValidationException("validation error"));

        Assert.ThrowsAsync<ValidationException>(() => _flashcardService.CreateFlashcards(dtos));
    }
}