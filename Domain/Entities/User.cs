namespace Domain.Entities;

#nullable disable

public sealed class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public int RoleId { get; set; }
    public int CourseId { get; set; }

    //Navigation
    public Role Role { get; set; }

    public Course Course { get; set; }

    public ICollection<Document> Documents { get; set; }
}