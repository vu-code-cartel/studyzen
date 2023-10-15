using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public class FlashcardEntityTypeConfiguration : IEntityTypeConfiguration<Flashcard>
{
    public void Configure(EntityTypeBuilder<Flashcard> builder)
    {
        builder.HasOne<FlashcardSet>()
            .WithMany()
            .HasForeignKey(f => f.FlashcardSetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(f => f.CreatedBy);

        builder.OwnsOne(f => f.UpdatedBy);
    }
}