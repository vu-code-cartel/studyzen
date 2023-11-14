using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using StudyZen.Application.Dtos;
using StudyZen.Domain.Entities;
using StudyZen.Domain.Enums;

namespace Api.IntegrationTests.controllers;

public class FlashcardsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly UpdateFlashcardDto _updateFlashcardDto;

    public FlashcardsControllerTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        _updateFlashcardDto = new UpdateFlashcardDto("New front", "New back");
    }
    /*
    [Fact]
    public async Task CreateFlashcard()
    {
        var flashcardSetRequest = new FlashcardSetDto(new FlashcardSet(null,"Set name",Color.Blue));
        var createSetResponse = await _httpClient.PostAsJsonAsync("FlashcardSets",flashcardSetRequest);
        Assert.Equal(HttpStatusCode.Created,createSetResponse.StatusCode);
        var flashcardSet = await createSetResponse.Content.ReadFromJsonAsync<FlashcardSetDto>();
        var createFlashcardDto = new CreateFlashcardDto(flashcardSet.Id, "New front", "New back");

        var response = await _httpClient.PostAsJsonAsync("Flashcards", createFlashcardDto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var flashcard = await response.Content.ReadFromJsonAsync<FlashcardDto>();
        Assert.NotNull(flashcard);
        Assert.Equal(createFlashcardDto.Front, flashcard.Front);
        Assert.Equal(createFlashcardDto.Back, flashcard.Back);
    }
    */
}


