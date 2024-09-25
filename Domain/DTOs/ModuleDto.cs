namespace Domain.DTOs;

public record ModuleDto(
    int Id,
    int CourseId,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    List<ActivityDto> Activities);
