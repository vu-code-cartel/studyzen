using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;
public sealed class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder
            .HasMany(q => q.PossibleAnswers)
            .WithOne()
            .HasForeignKey(a => a.QuizQuestionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(q => q.CorrectAnswer)
            .WithOne()
            .HasForeignKey<QuizQuestion>(q => q.CorrectAnswerId);
    }
}
