﻿using AutoFixture.NUnit3;
using FluentAssertions;
using StudyZen.Domain.Entities;
using StudyZen.Domain.ValueObjects;

namespace StudyZen.Domain.UnitTests.Entities;

public class BaseEntityTests
{
    private class BaseEntityWrapper : BaseEntity
    {
        public BaseEntityWrapper(int id) : base(id)
        {
        }
    }

    [Test]
    [AutoData]
    public void Constructor_Called_InstanceCreated(int id, UserActionStamp? createdBy, UserActionStamp? updatedBy)
    {
        var entity = new BaseEntityWrapper(id)
        {
            CreatedBy = createdBy,
            UpdatedBy = updatedBy
        };

        entity.Id.Should().Be(id);
        entity.CreatedBy.Should().Be(createdBy);
        entity.UpdatedBy.Should().Be(updatedBy);
    }
}