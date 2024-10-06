namespace Application.Models;

public class PaginationMeta
{
    public required int PageSize { get; init; }

    public required int TotalItemCount { get; init; }

    public int TotalPageCount => Convert.ToInt32(Math.Ceiling(TotalItemCount / Convert.ToDouble(PageSize)));
}
