namespace Domain.DTOs;

public record UserDto(
    string Name,
    string Username,
    string Email,
    int CourseId,
    CourseDto Course
);
