namespace Application.Interfaces;

public interface IServiceBase<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<TDto?> CreateAsync<TCreateDto>(TCreateDto dto);
    Task<TDto?> FindAsync(params object?[]? keyValues);
    Task<TDto> UpdateAsync<TUpdateDto>(object id, TUpdateDto updateDto);
    Task<bool> DeleteAsync(object id, TDto entityDto);
    Task<bool> DeleteAsync<T>(object id, T entityDto);
}
