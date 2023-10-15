using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public class FlashcardSetEntityTypeConfiguration : IEntityTypeConfiguration<FlashcardSet>
{
    public void Configure(EntityTypeBuilder<FlashcardSet> builder)
    {
        builder.HasOne<Lecture>()
            .WithMany()
            .HasForeignKey(fs => fs.LectureId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.OwnsOne(e => e.CreatedBy);

        builder.OwnsOne(e => e.UpdatedBy);
    }
}