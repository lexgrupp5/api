namespace Domain.DTOs;

#nullable disable

public class ActivityUpdateDto
{
    public int Id { get; init; }
    public string Description { get; init; }
    public DateTime? StartDate { get; init; } = null;
    public DateTime? EndDate { get; init; } = null;
}
