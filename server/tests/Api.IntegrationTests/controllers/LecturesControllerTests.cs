using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace Api.IntegrationTests.controllers;

[TestFixture]
public class LecturesControllerTests
{
    private HttpClient _httpClient;
    private UpdateLectureDto _updateLectureDto;
    private CourseDto _course;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        var factory = new WebApplicationFactory<Program>();
        _httpClient = factory.CreateClient();
        _updateLectureDto = new UpdateLectureDto("New name", "New cont");

        var createCourseDto = new CreateCourseDto("Test name", "Test desc");
        var courseResponse = await _httpClient.PostAsJsonAsync("Courses", createCourseDto);
        Assert.AreEqual(HttpStatusCode.Created, courseResponse.StatusCode);
        _course = await courseResponse.Content.ReadFromJsonAsync<CourseDto>();
    }

    [Test]
    public async Task CreateLecture()
    {
        var createLectureDto = new LectureDto(new Lecture(_course.Id, "Test name", "Test cont"));

        var response = await _httpClient.PostAsJsonAsync("Lectures", createLectureDto);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var lecture = await response.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(lecture);
        Assert.AreEqual(createLectureDto.Name, lecture.Name);
        Assert.AreEqual(createLectureDto.Content, lecture.Content);
    }

    [Test]
    public async Task GetLecture()
    {
        var createLectureDto = new LectureDto(new Lecture(_course.Id, "Test name", "Test cont"));
        var createResponse = await _httpClient.PostAsJsonAsync("Lectures", createLectureDto);
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
    public async Task UpdateLecture()
    {
        var createLectureDto = new LectureDto(new Lecture(_course.Id, "Test name", "Test cont"));
        var createResponse = await _httpClient.PostAsJsonAsync("Lectures", createLectureDto);
        Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        var newLecture = await createResponse.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(newLecture);

        var response = await _httpClient.PatchAsJsonAsync($"Lectures/{newLecture.Id}", _updateLectureDto);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"Lectures/{newLecture.Id}");
        var lecture = await getResponse.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(lecture);
        Assert.AreEqual(_updateLectureDto.Name, lecture.Name);
        Assert.AreEqual(_updateLectureDto.Content, lecture.Content);
    }

    [Test]
    public async Task DeleteLecture()
    {
        var createLectureDto = new LectureDto(new Lecture(_course.Id, "Test name", "Test cont"));
        var createResponse = await _httpClient.PostAsJsonAsync("Lectures", createLectureDto);
        Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode);
        var newLecture = await createResponse.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(newLecture);

        var response = await _httpClient.DeleteAsync($"Lectures/{newLecture.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"Lectures/{newLecture.Id}");
        Assert.AreEqual(HttpStatusCode.UnprocessableEntity, getResponse.StatusCode);
    }
}