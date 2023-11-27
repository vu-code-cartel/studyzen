using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using StudyZen.Api.Exceptions;
using StudyZen.Api.Extensions;
using System.Text.Json.Serialization;
using StudyZen.Application.Exceptions;
using System.Security.Authentication;
using StudyZen.Api.Auth;
using StudyZen.Application.Services;

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
                options.MapToStatusCode<InstanceNotFoundException>(StatusCodes.Status404NotFound);
                options.MapToStatusCode<UserAlreadyExistsException>(StatusCodes.Status422UnprocessableEntity);
                options.MapToStatusCode<AuthenticationException>(StatusCodes.Status401Unauthorized);
                options.MapToStatusCode<AccessDeniedException>(StatusCodes.Status403Forbidden);
                options.MapIdentifiableException();
            })
            .AddControllers()
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
            .AddProblemDetailsConventions()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddSignalR();

        ProblemDetailsExtensions.AddProblemDetails(services);

        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}