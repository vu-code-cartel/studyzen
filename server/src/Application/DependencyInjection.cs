using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using StudyZen.Application.Validators;
using StudyZen.Domain.Entities;

namespace StudyZen.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ILectureService, LectureService>();
        services.AddScoped<IFlashcardService, FlashcardService>();
        services.AddScoped<IFlashcardSetService, FlashcardSetService>();
        services.AddScoped<IValidator<CreateCourseDto>, CreateCourseRequestValidator>();
        services.AddScoped<IValidator<UpdateCourseDto>, UpdateCourseRequestValidator>();
        services.AddScoped<IValidator<CreateLectureDto>, CreateLectureRequestValidator>();
        services.AddScoped<IValidator<UpdateLectureDto>, UpdateLectureRequestValidator>();
        services.AddScoped<IValidator<CreateFlashcardDto>, CreateFlashcardRequestValidator>();
        services.AddScoped<IValidator<UpdateFlashcardDto>, UpdateFlashcardRequestValidator>();
        services.AddScoped<IValidator<CreateFlashcardDto>, CreateFlashcardRequestValidator>();
        services.AddScoped<IValidator<UpdateFlashcardSetDto>, UpdateFlashcardSetRequestValidator>();

        return services;
    }
}