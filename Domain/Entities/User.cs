using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

#nullable disable

public class User : IdentityUser
{
    public string Name { get; set; }
    
    public int CourseId { get; set; }

    //Navigation
    public Course Course { get; set; }
    public ICollection<Document> Documents { get; set; }
}