using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyZen.Domain.Entities;

namespace StudyZen.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private static readonly AuditableEntityInterceptor _interceptor = new AuditableEntityInterceptor();
    public DbSet<Lecture> Lectures { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Flashcard> Flashcards { get; set; } = null!;
    public DbSet<FlashcardSet> FlashcardSets { get; set; } = null!;
    public DbSet<Quiz> Quizzes { get; set; } = null!;
    public DbSet<QuizQuestion> QuizQuestions { get; set; } = null!;
    public DbSet<QuizAnswer> QuizAnswers { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _interceptor.SetHttpContextAccessor(httpContextAccessor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.AddInterceptors(_interceptor);
}