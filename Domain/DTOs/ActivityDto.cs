namespace Domain.DTOs;

public record ActivityDto(int Id, int ModuleId, string Description, DateTime StartDate, DateTime EndDate);