namespace Domain.DTOs;

public record ModuleToPatchDto(
    int Id,
    int CourseId,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate);