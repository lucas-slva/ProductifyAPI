using AutoMapper;
using Productify.Application.Contracts;
using Productify.Infrastructure.Contracts;

namespace Productify.Application.Services;

public class GenericService<TEntity, TDto, TCreateDto, TUpdateDto>(
    IGenericRepository<TEntity> repository,
    IMapper mapper) : IGenericService<TEntity, TDto, TCreateDto, TUpdateDto> where TEntity : class
{
    private readonly IGenericRepository<TEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<IEnumerable<TDto>> GetAllAsync(int skip, int take)
    {
        var entities = await _repository.GetRangeAsync(skip, take);
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }
    
    public async Task<TDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is not null ? _mapper.Map<TDto>(entity) : default;
    }

    public async Task<int> AddAsync(TCreateDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.AddAsync(entity);
        
        return (int)entity.GetType().GetProperty("Id")!.GetValue(entity)!;
    }

    public async Task UpdateAsync(TUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync((int)dto!.GetType().GetProperty("Id")!.GetValue(dto)!);
        if (entity == null) throw new Exception($"{typeof(TEntity).Name} not found");

        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            throw new InvalidOperationException("Category not found.");
    
        await _repository.DeleteAsync(entity);
    }
}