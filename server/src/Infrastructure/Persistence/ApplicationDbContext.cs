using System.Reflection;
using Microsoft.EntityFrameworkCore;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Lecture> Lectures { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Flashcard> Flashcards { get; set; } = null!;
    public DbSet<FlashcardSet> FlashcardSets { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}