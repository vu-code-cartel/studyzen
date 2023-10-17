using AutoFixture.NUnit3;
using FluentAssertions;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Domain.UnitTests.ValueObjects;

public class UserActionStampTests
{
    [Test]
    [AutoData]
    public void Constructor_Called_InstanceCreated(string user, DateTime timestamp)
    {
        var actualStamp = new UserActionStamp(user, timestamp);

        actualStamp.User.Should().Be(user);
        actualStamp.Timestamp.Should().Be(timestamp);
    }
}