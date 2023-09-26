﻿using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Repositories;
using StudyZen.Infrastructure.Repositories;
using StudyZen.Infrastructure.Services;

namespace StudyZen.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ILectureRepository, LectureRepository>();
        services.AddScoped<IFlashcardRepository, FlashcardRepository>();
        services.AddScoped<IFlashcardSetRepository, FlashcardSetRepository>();

        services.AddSingleton<IFileService, FileService>();

        return services;
    }
}