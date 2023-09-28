using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Services;

namespace StudyZen.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ILectureService, LectureService>();
        services.AddScoped<IFlashcardService, FlashcardService>();
        services.AddScoped<IFlashcardSetService, FlashcardSetService>();

        return services;
    }
}