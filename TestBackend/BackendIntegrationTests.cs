using System.Net;
using Infrastructure.Persistence;

namespace TestBackend;

public class BackendIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private AppDbContext _context;
    
    public BackendIntegrationTests(CustomWebApplicationFactory clientFactory)
    {
        clientFactory.ClientOptions.BaseAddress = new Uri("https://localhost:5144");
        _httpClient = clientFactory.CreateClient();
        _context = clientFactory.Context;
    }
    
    [Fact]
    public async Task GetUsersAsync_ReturnsOkResult()
    {
        
        //Arrange
        var expectedStatusCode = HttpStatusCode.OK;
        var expectedContentType = "application/json";
        
        //Act
        using var response = await _httpClient.GetAsync("api/User");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
    
        //Assert
        Assert.IsType<string>(responseBody);
        Assert.NotEqual(string.Empty, responseBody);
        Assert.Equal(expectedStatusCode, response.StatusCode);
        Assert.Contains(expectedContentType, response.Content.Headers.ContentType.ToString());

    }
   
}