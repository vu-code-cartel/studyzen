using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using StudyZen.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using StudyZen.Application.Authentication;
using StudyZen.Infrastructure.Authentication;

namespace StudyZen.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ILectureRepository, LectureRepository>();
        services.AddScoped<IFlashcardRepository, FlashcardRepository>();
        services.AddScoped<IFlashcardSetRepository, FlashcardSetRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}