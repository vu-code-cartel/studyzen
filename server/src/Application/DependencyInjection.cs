﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Services;
using StudyZen.Application.Validation;
using StudyZen.Application.Validation.Validators;


namespace StudyZen.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ILectureService, LectureService>();
        services.AddScoped<IFlashcardService, FlashcardService>();
        services.AddScoped<IFlashcardSetService, FlashcardSetService>();
        services.AddValidatorsFromAssemblyContaining<CreateLectureRequestValidator>();
        services.AddScoped<ValidationHandler>();

        return services;
    }
}