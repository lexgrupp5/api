using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Service;

public class ServiceCoordinator(
    Lazy<IExampleService> exampleService,
    Lazy<UserManager<User>> userManager
) : IServiceCoordinator
{
    private readonly Lazy<IExampleService> _exampleService = exampleService;

    private readonly Lazy<UserManager<User>> _userManager = userManager;

    public IExampleService Examples => _exampleService.Value;

    public UserManager<User> User => _userManager.Value;
}
