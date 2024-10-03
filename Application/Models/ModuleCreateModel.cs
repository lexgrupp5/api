namespace Application.Models;

public record ModuleCreateModel(string Name, int CourseId, string Description, DateTime StartDate, DateTime EndDate);