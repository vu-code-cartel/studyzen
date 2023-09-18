using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using StudyZen.Common.Exceptions;
using StudyZen.Courses;
using StudyZen.FlashCards;
using StudyZen.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddProblemDetails(options =>
    {
        options.MapToStatusCode<RequestArgumentNullException>(StatusCodes.Status400BadRequest);
        options.MapToStatusCode<InstanceNotFoundException>(StatusCodes.Status404NotFound);
    })
    .AddControllers()
    .AddProblemDetailsConventions()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

ProblemDetailsExtensions.AddProblemDetails(builder.Services);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IFlashCardService, FlashcardService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();