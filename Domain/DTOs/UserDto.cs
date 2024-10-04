namespace Domain.DTOs;

#nullable disable

public record UserDto
{
    public string Name { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
    public int CourseId { get; init; }

    // Course
    public CourseDto Course { get; init; }
};
