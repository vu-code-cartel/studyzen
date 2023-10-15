using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Repositories;
using StudyZen.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StudyZen.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;

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

        services.AddSingleton<IFileService, FileService>();

        return services;
    }
}