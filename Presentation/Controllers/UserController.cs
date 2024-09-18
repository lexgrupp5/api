
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Service;

public class UserController(IServiceCoordinator serviceCordinator) : ControllerBase
{
    private readonly IServiceCoordinator _sc = serviceCordinator;

}