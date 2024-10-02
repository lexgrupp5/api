namespace Domain.DTOs;

#nullable disable

public class CourseCompleteDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string TeacherId { get; set; }

    public ICollection<ModuleDto> Modules { get; set; }
    public ICollection<UserDto> Students { get; set; }
}