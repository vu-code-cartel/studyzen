using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence.EntityTypeConfigurations;

public class FlashcardEntityTypeConfiguration : IEntityTypeConfiguration<Flashcard>
{
    public void Configure(EntityTypeBuilder<Flashcard> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd().HasColumnName("id");
        builder.HasOne<Lecture>().WithMany().HasForeignKey(f => f.FlashcardSetId).HasConstraintName("flashcardset_id").OnDelete(DeleteBehavior.Cascade);
        builder.Property(f => f.Question).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("question");
        builder.Property(f => f.Answer).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("answer");
        builder.Property(f => f.CreatedBy.User).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("createdby_user");
        builder.Property(f => f.CreatedBy.Timestamp).IsRequired().HasColumnType("DATETIME2").HasColumnName("createdby_timestamp");
        builder.Property(f => f.UpdatedBy.User).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50).HasColumnName("updatedby_user");
        builder.Property(f => f.UpdatedBy.Timestamp).IsRequired().HasColumnType("DATETIME2").HasColumnName("updatedby_timestamp");
    }
}