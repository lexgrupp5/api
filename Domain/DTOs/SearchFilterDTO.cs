namespace Domain.DTOs;

public class SearchFilterDTO
{
    public string SearchText { get; set; } = string.Empty;
    public DateTime EndDate {  get; set; }
    public DateTime StartDate { get; set; }
}
