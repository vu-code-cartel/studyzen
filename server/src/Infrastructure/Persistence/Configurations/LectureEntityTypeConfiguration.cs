using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public class LectureEntityTypeConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(l => l.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}