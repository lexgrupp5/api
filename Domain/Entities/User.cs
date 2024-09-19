using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

#nullable disable

public class User : IdentityUser
{
    /* public int Id { get; set; } */

    public string Name { get; set; }

    public int CourseId { get; set; }

    //public string Role {  get; set; }

    //public string Password { get; set; }

    //Navigation

    public Course Course { get; set; }

    public ICollection<Document> Documents { get; set; }
}