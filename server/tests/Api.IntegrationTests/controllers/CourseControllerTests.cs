using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.setup;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyZen.Application.Dtos;
using StudyZen.Infrastructure.Persistence;

namespace Api.IntegrationTests.controllers;

[TestFixture]
public class CourseControllerTests
{
    private HttpClient _httpClient;
    private WebApplicationFactory<Program> _factory;
    private CreateCourseDto _createCourseDto;
    private UpdateCourseDto _updateCourseDto;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = TestFactory.GetNewTestFactory();
        _httpClient = _factory.CreateClient();
        _createCourseDto = new CreateCourseDto("Name", "Description");
        _updateCourseDto = new UpdateCourseDto("New name", "New desc");
    }

    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        _httpClient.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task CreateCourse_ValidInput_ReturnsCreated()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var response = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Name == _createCourseDto.Name);
        Assert.NotNull(course);
        Assert.AreEqual(_createCourseDto.Description, course.Description);
    }

    [Test]
    public async Task GetCourse_ValidInput_ReturnsOk()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var course = await response.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.AreEqual(newCourse.Name, course.Name);
        Assert.AreEqual(newCourse.Description, course.Description);
    }

    [Test]
    public async Task GetAllCourses_ValidRequest_ReturnsOk()
    {
        var response = await _httpClient.GetAsync("Courses");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var allCourses = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<CourseDto>>();
        Assert.NotNull(allCourses);
    }

    [Test]
    public async Task UpdateCourse_ValidInput_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.PatchAsJsonAsync($"Courses/{newCourse.Id}", _updateCourseDto);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == newCourse.Id);
        Assert.NotNull(course);
        Assert.AreEqual(_updateCourseDto.Name, course.Name);
        Assert.AreEqual(_updateCourseDto.Description, course.Description);
    }

    [Test]
    public async Task DeleteCourse_ValidRequest_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.DeleteAsync($"Courses/{newCourse.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == newCourse.Id);
        Assert.IsNull(course);
    }
}