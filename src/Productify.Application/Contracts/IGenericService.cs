namespace Productify.Application.Contracts;

public interface IGenericService<TEntity, TDto, in TCreateDto, in TUpdateDto> where TEntity : class
{
    Task<IEnumerable<TDto>> GetAllAsync(int skip, int take);
    Task<TDto?> GetByIdAsync(int id);
    Task<int> AddAsync(TCreateDto dto);
    Task UpdateAsync(TUpdateDto dto);
    Task DeleteAsync(int id);
}