using Application.Interfaces;

using Domain.DTOs;
using Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Presentation.Controllers;

namespace TestBackend;

public class BackendTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IServiceCoordinator> _mockServiceCoordinator;


    public BackendTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockServiceCoordinator = new Mock<IServiceCoordinator>();
        _mockServiceCoordinator.Setup(s => s.User).Returns(_mockUserService.Object);
    }
    
    [Fact]
    public async Task CreateNewUserAsync_ReturnsOkResult_WithCreatedUser()
    {
        //Arrange
        var testCourseDto = new CourseDto {Description = "Test-desc", Name = "Test-name", StartDate = DateTime.Today, EndDate = DateTime.Today, TeacherId = "1", Id = 1};
        var newUser = new UserForCreationDto { Name = "Tom", Email = "tom@mail.com" , Username = "Test"};
        var expectedUser = new UserDto(newUser.Name, newUser.Email, newUser.Username, 1, testCourseDto );
        
        _mockUserService.Setup(u => u.CreateNewUserAsync(It.IsAny<UserForCreationDto>(),
            It.IsAny<UserManager<User>>(),
            It.IsAny<IIdentityService>()))
            .ReturnsAsync(expectedUser);
            
        
        var controller = new UserController(_mockServiceCoordinator.Object);

        //Act
        var result = await controller.CreateNewUserAsync(newUser);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
        if (result.Result is OkObjectResult okResult)
        {
            Assert.NotNull(okResult.Value);
        
            if (okResult.Value is UserDto createdUser)
            {
                Assert.Equal(expectedUser.Name, createdUser.Name);
                Assert.Equal(expectedUser.Email, createdUser.Email);
                Assert.Equal(expectedUser.Username, createdUser.Username);
            }
            else
            {
                Assert.Fail("Expected UserDto, got null or unexpected type");
            }
        }
        else
        {
            Assert.Fail("Expected OkObjectResult, but got a different result type");
        }

        //Verify service call
        _mockUserService.Verify(u => u.CreateNewUserAsync(It.IsAny<UserForCreationDto>(), 
            It.IsAny<UserManager<User>>(), 
            It.IsAny<IIdentityService>()), Times.Once());
   
    }

}