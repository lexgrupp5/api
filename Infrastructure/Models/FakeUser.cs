using Domain.Entities;

namespace Infrastructure.Models;

#nullable disable

public class FakeUser
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Course Course { get; set; }
    public string RoleName { get; set; }
}
