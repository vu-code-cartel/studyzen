﻿using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using StudyZen.Api.Exceptions;
using StudyZen.Api.Extensions;
using System.Text.Json.Serialization;

namespace StudyZen.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        bool? isDevEnv = null;

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy => policy
                .WithOrigins(
                    "http://127.0.0.1:5173",
                    "http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        services
            .AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (_, _) =>
                {
                    if (isDevEnv == null)
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        var env = serviceProvider.GetService<IWebHostEnvironment>();
                        isDevEnv = env?.IsDevelopment() ?? false;
                    }

                    return isDevEnv.Value;
                };
                options.MapToStatusCode<RequestArgumentNullException>(StatusCodes.Status400BadRequest);
                options.MapValidationException();
            })
            .AddControllers()
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
            .AddProblemDetailsConventions()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        ProblemDetailsExtensions.AddProblemDetails(services);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}