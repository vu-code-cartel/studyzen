using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;

namespace Api.IntegrationTests.controllers;

public class LecturesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly UpdateLectureDto _updateLectureDto;

    public LecturesControllerTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        _updateLectureDto = new UpdateLectureDto("New name", "New cont");
    }

    private async Task<CourseDto> CreateCourse()
    {
        var createCourseDto = new CreateCourseDto("Test name", "Test desc");
        var courseResponse = await _httpClient.PostAsJsonAsync("Courses", createCourseDto);
        Assert.Equal(HttpStatusCode.Created, courseResponse.StatusCode);
        var course = await courseResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        return course;
    }

    [Fact]
    public async Task CreateLecture()
    {
        var course = CreateCourse().Result;
        var createLectureDto = new LectureDto(new Lecture(course.Id,"Test name","Test cont"));

        var response = await _httpClient.PostAsJsonAsync("Lectures", createLectureDto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var lecture = await response.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(lecture);
        Assert.Equal(createLectureDto.Name, lecture.Name);
        Assert.Equal(createLectureDto.Content, lecture.Content);
    }

    [Fact]
    public async Task GetLecture()
    {
        var course = CreateCourse().Result;
        var createLectureDto = new LectureDto(new Lecture(course.Id, "Test name", "Test cont"));
        var createResponse = await _httpClient.PostAsJsonAsync("Lectures", createLectureDto);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var newLecture = await createResponse.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(newLecture);

        var response = await _httpClient.GetAsync($"Lectures/{newLecture.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var lecture = await response.Content.ReadFromJsonAsync<LectureDto>();
        Assert.NotNull(lecture);
        Assert.Equal(newLecture.Name, lecture.Name);
        Assert.Equal(newLecture.Content, lecture.Content);
    }


}

