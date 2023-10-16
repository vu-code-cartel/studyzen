using AutoFixture.NUnit3;
using FluentAssertions;
using StudyZen.Domain.Entities;

namespace StudyZen.Domain.UnitTests.Entities;

public class CourseTests
{
    [Test]
    [AutoData]
    public void Constructor_Called_InstanceCreated(string name, string description)
    {
        var actualCourse = new Course(name, description);

        actualCourse.Id.Should().Be(default);
        actualCourse.Name.Should().Be(name);
        actualCourse.Description.Should().Be(description);
        actualCourse.CreatedBy.Should().Be(null);
        actualCourse.UpdatedBy.Should().Be(null);
    }
}