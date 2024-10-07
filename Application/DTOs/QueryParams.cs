using Infrastructure.Models;

namespace Application.DTOs;

public class QueryParams
{
    public int? Page { get; set; } = 1;
    public int? Limit { get; set; } = 100;
    public List<SortParams>? SortFields { get; set; } = [];
}
