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
public class LecturesControllerTests
{
    private HttpClient _httpClient;
    private WebApplicationFactory<Program> _factory;
    private CreateLectureDto _createLectureDto;
    private UpdateLectureDto _updateLectureDto;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _factory = TestFactory.GetNewTestFactory();
        _httpClient = _factory.CreateClient();
        _updateLectureDto = new UpdateLectureDto("New name", "New cont");

        var createCourseDto = new CreateCourseDto("Test name", "Test desc");
        var courseResponse = await _httpClient.PostAsJsonAsync("Courses", createCourseDto);
        var course = await courseResponse.Content.ReadFromJsonAsync<CourseDto>();
        _createLectureDto = new CreateLectureDto(course.Id, "Test name", "Test cont");
    }

    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        _httpClient.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task CreateLecture_ValidInput_ReturnsCreated()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var response = await _httpClient.PostAsJsonAsync("Lectures", _createLectureDto);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var lecture = await dbContext.Lectures.FirstOrDefaultAsync(c => c.Name == _createLectureDto.Name);
        Assert.NotNull(lecture);
        Assert.AreEqual(_createLectureDto.CourseId, lecture.CourseId);
        Assert.AreEqual(_createLectureDto.Content, lecture.Content);
    }

    [Test]
    public async Task GetLecture_ValidInput_ReturnsOk()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Lectures", _createLectureDto);
        Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        var newLecture = await createResponse.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(newLecture);

        var response = await _httpClient.GetAsync($"Lectures/{newLecture.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var lecture = await response.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(lecture);
        Assert.AreEqual(newLecture.Name, lecture.Name);
        Assert.AreEqual(newLecture.Content, lecture.Content);
    }

    [Test]
    public async Task UpdateLecture_ValidInput_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createResponse = await _httpClient.PostAsJsonAsync("Lectures", _createLectureDto);
        var newLecture = await createResponse.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(newLecture);

        var response = await _httpClient.PatchAsJsonAsync($"Lectures/{newLecture.Id}", _updateLectureDto);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var lecture = await dbContext.Lectures.FirstOrDefaultAsync(c => c.Id == newLecture.Id);
        Assert.NotNull(lecture);
        Assert.AreEqual(_updateLectureDto.Name, lecture.Name);
        Assert.AreEqual(_updateLectureDto.Content, lecture.Content);
    }

    [Test]
    public async Task DeleteLecture_ValidRequest_ReturnsOk()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var createResponse = await _httpClient.PostAsJsonAsync("Lectures", _createLectureDto);
        Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        var newLecture = await createResponse.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(newLecture);

        var response = await _httpClient.DeleteAsync($"Lectures/{newLecture.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var lecture = await dbContext.Lectures.FirstOrDefaultAsync(c => c.Id == newLecture.Id);
        Assert.IsNull(lecture);
    }
}