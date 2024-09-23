using Domain.Validations;

namespace Domain.DTOs;

public class SearchFilterDTO
{
    [SearchTextValidation]
    public string SearchText { get; set; } = string.Empty;

    public DateTime EndDate {  get; set; }

    public DateTime StartDate { get; set; }
}
