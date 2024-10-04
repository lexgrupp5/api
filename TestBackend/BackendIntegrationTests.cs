using System.Net;
using System.Net.Http.Json;
using Domain.DTOs;

using Newtonsoft.Json;

namespace TestBackend;

public class BackendIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public BackendIntegrationTests(CustomWebApplicationFactory clientFactory)
    {
        clientFactory.ClientOptions.BaseAddress = new Uri("https://localhost:5144");
        _httpClient = clientFactory.CreateClient();
    }

    [Fact]
    public async Task GetNotExistingUsernameAsync_ReturnsBadResultAndNull()
    {
        //Arrange
        var expectedStatusCode = HttpStatusCode.BadRequest;
        string userName = "testUser";

        //Act
        using var response = await _httpClient.GetAsync($"/api/users/{userName}");
        var errorMessage = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Equal(expectedStatusCode, response.StatusCode);
        Assert.Contains("A user with that username was not able to be found.", errorMessage);
        
    }

    
    [Fact]
    public async Task CreateUserAsync_ReturnsValidUserDto()
    {
        //Arrange
        var newUser = new UserCreateDto{ Name = "Test", Email = "test@test.com", Username = "test", Password = "Test"};

        //Act
        using var response = await _httpClient.PostAsJsonAsync("/api/users", newUser);
         response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();
        var deserializedResult = JsonConvert.DeserializeObject<UserDto>(result);
        
        //Assert
        Assert.IsType<UserDto>(deserializedResult);
        Assert.NotNull(deserializedResult);
        Assert.Equal(newUser.Name, deserializedResult.Name);
        Assert.Equal(newUser.Email, deserializedResult.Email);
        Assert.Equal(newUser.Username, deserializedResult.Username);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetModuleWithIdAsync_ReturnsOkResultAndNotNullResult()
    {
        //Arrange
        int id = 1;
        
        //Act
        using var response = await _httpClient.GetAsync($"/api/modules/{id}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData(1, 200)]
    [InlineData(2, 200)]
    [InlineData(3, 200)]
    public async Task GetCoursesAsync_ReturnsOkResultAndNotNullResult(int id, int expectedStatusCode)
    {
        //Arrange & Act
        using var response = await _httpClient.GetAsync($"/api/courses/{id}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();

        
        //Assert
        Assert.Equal(expectedStatusCode, (int)response.StatusCode);
        Assert.NotNull(result);
    }   
    [Fact]
    public async Task GetActivitiesAsync_ReturnsOkResult()
    {
        //Arrange
        int id = 1;
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        
        // Act
        using var response = await _httpClient.GetAsync($"/api/activities/{id}");
        response.EnsureSuccessStatusCode();
        
        //Assert
        Assert.Equal(expectedStatusCode, response.StatusCode);

    }
}