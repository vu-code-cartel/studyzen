using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Testing;
using StudyZen.Application.Dtos;
using StudyZen.Infrastructure.Persistence;


namespace Api.IntegrationTests.controllers;

[TestFixture]
public class CourseControllerTests
{
    private HttpClient _httpClient;
    private CreateCourseDto _createCourseDto;
    private UpdateCourseDto _updateCourseDto;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _httpClient = TestClient.GetTestClient();
        _createCourseDto = new CreateCourseDto("Test name", "Test desc");
        _updateCourseDto = new UpdateCourseDto("New name", "New desc");
    }

    [OneTimeTearDown]
    public async Task OneTimeTeardown()
    {
        _httpClient.Dispose();
    }


    [Test]
    public async Task CreateCourse_ValidInput_ReturnsCreated()
    {
        var response = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

        var course = await response.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.AreEqual(_createCourseDto.Name, course.Name);
        Assert.AreEqual(_createCourseDto.Description, course.Description);
    }

    [Test]
    public async Task GetCourse_ValidInput_ReturnsOk()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
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
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.PatchAsJsonAsync($"Courses/{newCourse.Id}", _updateCourseDto);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        var course = await getResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(course);
        Assert.AreEqual(_updateCourseDto.Name, course.Name);
        Assert.AreEqual(_updateCourseDto.Description, course.Description);
    }

    [Test]
    public async Task DeleteCourse_ValidRequest_ReturnsOk()
    {
        var createResponse = await _httpClient.PostAsJsonAsync("Courses", _createCourseDto);
        var newCourse = await createResponse.Content.ReadFromJsonAsync<CourseDto>();
        Assert.NotNull(newCourse);

        var response = await _httpClient.DeleteAsync($"Courses/{newCourse.Id}");
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var getResponse = await _httpClient.GetAsync($"Courses/{newCourse.Id}");
        Assert.AreEqual(HttpStatusCode.UnprocessableEntity, getResponse.StatusCode);
    }
}