using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence.EntityTypeConfigurations;

public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedOnAdd().HasColumnName("id");
        builder.Property(c => c.Name).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("name");
        builder.Property(c => c.Description).IsRequired().HasColumnType("VARCHAR").HasMaxLength(250).HasColumnName("description");
        builder.Property(c => c.CreatedBy.User).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("createdby_user");
        builder.Property(c => c.CreatedBy.Timestamp).IsRequired().HasColumnType("DATETIME2").HasColumnName("createdby_timestamp");
        builder.Property(c => c.UpdatedBy.User).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("updatedby_user");
        builder.Property(c => c.UpdatedBy.Timestamp).IsRequired().HasColumnType("DATETIME2").HasColumnName("updatedby_timestamp");
    }
}