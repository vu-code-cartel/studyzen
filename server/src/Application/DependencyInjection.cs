using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Services;
using StudyZen.Application.Validation;

namespace StudyZen.Application;

public static class DependencyInjection
{
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
                services.AddScoped<ICourseService, CourseService>();
                services.AddScoped<ILectureService, LectureService>();
                services.AddScoped<IFlashcardService, FlashcardService>();
                services.AddScoped<IFlashcardSetService, FlashcardSetService>();
                services.AddScoped<IQuizService, QuizService>();
                services.AddScoped<IApplicationUserService, ApplicationUserService>();
                services.AddScoped<IDataImporter, CsvDataImporter>();
                services.AddScoped<IFlashcardImporter, FlashcardImporter>();
                services.AddScoped<ValidationHandler>();

                services.AddValidatorsFromAssemblyContaining<CreateLectureRequestValidator>();

                return services;
        }
}