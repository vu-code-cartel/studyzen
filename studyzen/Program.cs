using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using StudyZen.Common.Exceptions;
using StudyZen.Courses;
using StudyZen.Lectures;
using StudyZen.Flashcards;
using StudyZen.FlashcardSets;
using StudyZen.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy
        .WithOrigins(
            "http://127.0.0.1:5173",
            "http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services
    .AddProblemDetails(options => { options.MapToStatusCode<RequestArgumentNullException>(StatusCodes.Status400BadRequest); })
    .AddControllers()
    .AddProblemDetailsConventions()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

ProblemDetailsExtensions.AddProblemDetails(builder.Services);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ILectureService, LectureService>();
builder.Services.AddScoped<IFlashcardService, FlashcardService>();
builder.Services.AddScoped<IFlashcardSetService, FlashcardSetService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails();
app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();