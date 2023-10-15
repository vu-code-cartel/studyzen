using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.OwnsOne(l => l.CreatedBy);

        builder.OwnsOne(l => l.UpdatedBy);
    }
}