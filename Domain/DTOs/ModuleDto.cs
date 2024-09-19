namespace Domain.DTOs;

public record ModuleDto(
    int Id,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    List<ActivityDto> Activities);
