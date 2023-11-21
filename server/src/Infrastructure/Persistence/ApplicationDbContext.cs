using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Services;
using StudyZen.Domain.Entities;
using System.Reflection;

namespace StudyZen.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public DbSet<Lecture> Lectures { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Flashcard> Flashcards { get; set; } = null!;
    public DbSet<FlashcardSet> FlashcardSets { get; set; } = null!;
    public DbSet<Quiz> Quizzes { get; set; } = null!;
    public DbSet<QuizQuestion> QuizQuestions { get; set; } = null!;
    public DbSet<QuizAnswer> QuizAnswers { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options, 
        ICurrentUserAccessor currentUserAccessor) : base(options)
    {
        _currentUserAccessor = currentUserAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(new AuditableEntityInterceptor(_currentUserAccessor));
}