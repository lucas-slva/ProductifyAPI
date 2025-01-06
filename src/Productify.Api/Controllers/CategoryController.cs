using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productify.Application.Contracts;
using Productify.Application.DTOs;
using Productify.Domain.Entities;

namespace Productify.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(IGenericService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto> service) : ControllerBase
{
    private readonly IGenericService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto> _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAsync([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var categories = await _service.GetAllAsync(skip, take);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var category = await _service.GetByIdAsync(id);
        return category is null 
            ? NotFound($"Category not found.") 
            : Ok(category);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Category name cannot be empty.");
        
        var generatedId = await _service.AddAsync(dto);
        return CreatedAtAction("GetById", new { id = generatedId }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch.");
        
        var existingCategory = await _service.GetByIdAsync(id);
        if (existingCategory == null)
            return NotFound($"Category not found.");
        
        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null)
            return NotFound($"Category not found.");
    
        await _service.DeleteAsync(id);
        return NoContent();
    }
}