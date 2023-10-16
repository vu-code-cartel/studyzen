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

        actualCourse.Name.Should().Be(name);
        actualCourse.Description.Should().Be(description);
    }
}