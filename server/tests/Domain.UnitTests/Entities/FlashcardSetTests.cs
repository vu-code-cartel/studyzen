using AutoFixture.NUnit3;
using FluentAssertions;
using StudyZen.Domain.Entities;
using StudyZen.Domain.Enums;

namespace StudyZen.Domain.UnitTests.Entities;

public class FlashcardSetTests
{
    [Test]
    [AutoData]
    public void Constructor_Called_InstanceCreated(int? lectureId, string name, Color color)
    {
        var actualFlashcardSet = new FlashcardSet(lectureId, name, color);

        actualFlashcardSet.LectureId.Should().Be(lectureId);
        actualFlashcardSet.Name.Should().Be(name);
        actualFlashcardSet.Color.Should().Be(color);
    }
}