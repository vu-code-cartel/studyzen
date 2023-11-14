using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using StudyZen.Application.Dtos;

namespace Api.IntegrationTests.controllers;
public class CourseControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CourseControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCourse()
    {
        var client = _factory.CreateClient();
        var courseId = 1;

        var response = await client.GetAsync($"Courses/{courseId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var course = await response.Content.ReadFromJsonAsync<CourseDto>();

        Assert.NotNull(course);
    }
}

