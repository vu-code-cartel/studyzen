using Microsoft.EntityFrameworkCore;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<FlashcardSet> FlashcardSets { get; set; }

}