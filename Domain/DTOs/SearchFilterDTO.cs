using Domain.Validations;

namespace Domain.DTOs;

public class SearchFilterDTO
{
    [SearchTextValidation]
    public string SearchText { get; set; } = string.Empty;

    [DomainDateValidation]
    public DateTime EndDate {  get; set; }

    [DomainDateValidation]
    public DateTime StartDate { get; set; }
}
