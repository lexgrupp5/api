namespace Domain.DTOs;

#nullable disable

public class CourseCompleteDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string TeacherId { get; init; }

    // Module
    public ICollection<ModuleDto> Modules { get; init; }

    // User
    public ICollection<UserDto> Students { get; init; }
}