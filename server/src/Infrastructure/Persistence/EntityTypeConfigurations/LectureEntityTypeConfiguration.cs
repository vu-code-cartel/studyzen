using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence.EntityTypeConfigurations;

public class LectureEntityTypeConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedOnAdd().HasColumnName("id");
        builder.HasOne<Course>().WithMany().HasForeignKey(l => l.CourseId).IsRequired().HasConstraintName("course_id").OnDelete(DeleteBehavior.Cascade);
        builder.Property(l => l.Name).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("name");
        builder.Property(l => l.Content).IsRequired().HasColumnType("TEXT").HasColumnName("content");
        builder.Property(l => l.CreatedBy.User).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("createdby_user");
        builder.Property(l => l.CreatedBy.Timestamp).IsRequired().HasColumnType("DATETIME2").HasColumnName("createdby_timestamp");
        builder.Property(l => l.UpdatedBy.User).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("updatedby_user");
        builder.Property(l => l.UpdatedBy.Timestamp).IsRequired().HasColumnType("DATETIME2").HasColumnName("updatedby_timestamp");
    }
}