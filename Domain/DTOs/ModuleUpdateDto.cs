namespace Domain.DTOs;

public record ModuleUpdateDto
{
    public int Id { get; init; }
    public string? Name { get; init; } = null;
    public string? Description { get; init; } = null;
}
