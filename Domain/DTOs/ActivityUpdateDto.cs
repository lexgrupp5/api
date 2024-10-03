namespace Domain.DTOs;

#nullable disable

public class ActivityUpdateDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
