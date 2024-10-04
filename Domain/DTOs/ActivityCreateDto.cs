namespace Domain.DTOs;

#nullable disable

public record ActivityCreateDto
{
    public string Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    // Module
    public int ModuleId { get; init; }

    // Activity Type
    public string ActivityTypeName { get; init; }
}
