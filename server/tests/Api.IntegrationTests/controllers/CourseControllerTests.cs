using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using StudyZen.Application.Dtos;

namespace Api.IntegrationTests.controllers;

public class CourseControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly CreateCourseDto _createCourseDto;
    private readonly UpdateCourseDto _updateCourseDto;

    public CourseControllerTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        _createCourseDto = new CreateCourseDto("Test name", "Test desc");
        _updateCourseDto = new UpdateCourseDto("New name", "New desc");
    }

    [Fact]
    public async Task CreateCourse()
    {
        var response = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var course = await response.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.Equal(_createCourseDto.Name, course.Name);
        Assert.Equal(_createCourseDto.Description, course.Description);
    }

    [Fact]
    public async Task GetCourse()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var course = await response.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.Equal(newCourse.Name, course.Name);
        Assert.Equal(newCourse.Description, course.Description);
    }

    [Fact]
    public async Task GetAllCourses()
    {
        var response = await _httpClient.GetAsync("Courses");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var allCourses = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<CourseDto>>();
        Assert.NotNull(allCourses);
    }

    [Fact]
    public async Task UpdateCourse()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.PatchAsJsonAsync($"Courses/{newCourse.Id}", _updateCourseDto);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        var course = await getResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.Equal(_updateCourseDto.Name, course.Name);
        Assert.Equal(_updateCourseDto.Description, course.Description);
    }

    [Fact]
    public async Task DeleteCourse()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.DeleteAsync($"Courses/{newCourse.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        Assert.Equal(HttpStatusCode.UnprocessableEntity, getResponse.StatusCode);
    }
}

