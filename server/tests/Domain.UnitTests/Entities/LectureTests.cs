using AutoFixture.NUnit3;
using FluentAssertions;
using StudyZen.Domain.Entities;

namespace StudyZen.Domain.UnitTests.Entities;

public class LectureTests
{
    [Test]
    [AutoData]
    public void Constructor_Called_InstanceCreated(int courseId, string name, string content)
    {
        var actualLecture = new Lecture(courseId, name, content);

        actualLecture.Id.Should().Be(default);
        actualLecture.CourseId.Should().Be(courseId);
        actualLecture.Name.Should().Be(name);
        actualLecture.Content.Should().Be(content);
        actualLecture.CreatedBy.Should().Be(null);
        actualLecture.UpdatedBy.Should().Be(null);
    }
}