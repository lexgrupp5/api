using Infrastructure.Models;

namespace Application.Helpers;

public class PageParamsValidator
{
    private readonly int _minPageSize = 10;
    private readonly int _maxPageSize = 500;

    public PageParamsValidator() { }

    public PageParams Adjust(PageParams pageParams) =>
        new()
        {
            Page = Math.Max(pageParams.Page, 1),
            Size = Math.Clamp(pageParams.Size, _minPageSize, _maxPageSize)
        };
}
