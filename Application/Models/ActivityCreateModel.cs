namespace Application.Models;

public record ActivityCreateModel(int ModuleId,string Description, DateTime StartDate, DateTime EndDate);