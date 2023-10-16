using AutoFixture.NUnit3;
using FluentAssertions;
using StudyZen.Domain.Entities;

namespace StudyZen.Domain.UnitTests.Entities;

public class FlashcardTests
{
    [Test]
    [AutoData]
    public void Constructor_Called_InstanceCreated(int flashcardSetId, string front, string back)
    {
        var actualFlashcard = new Flashcard(flashcardSetId, front, back);

        actualFlashcard.FlashcardSetId.Should().Be(flashcardSetId);
        actualFlashcard.Front.Should().Be(front);
        actualFlashcard.Back.Should().Be(back);
    }
}