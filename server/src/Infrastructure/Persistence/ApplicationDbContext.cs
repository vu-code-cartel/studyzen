using Microsoft.EntityFrameworkCore;
using StudyZen.Domain.Entities;
using StudyZen.Infrastructure.Persistence.EntityTypeConfigurations;

namespace StudyZen.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new CourseEntityTypeConfiguration().Configure(modelBuilder.Entity<Course>());
        new LectureEntityTypeConfiguration().Configure(modelBuilder.Entity<Lecture>());
        new FlashcardSetEntityTypeConfiguration().Configure(modelBuilder.Entity<FlashcardSet>());
        new FlashcardEntityTypeConfiguration().Configure(modelBuilder.Entity<Flashcard>());
    }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<FlashcardSet> FlashcardSets { get; set; }

}