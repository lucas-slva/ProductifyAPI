using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productify.Application.Contracts;
using Productify.Application.DTOs;
using Productify.Domain.Entities;

namespace Productify.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IGenericService<Product, ProductDto, CreateProductDto, UpdateProductDto> service) : ControllerBase
{
    private readonly IGenericService<Product, ProductDto, CreateProductDto, UpdateProductDto> _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetRangeAsync([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        if (skip < 0 || take <= 0)
            return BadRequest("Invalid pagination parameters.");

        var products = await _service.GetAllAsync(skip, take);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        if (id <= 0) return BadRequest("Invalid product ID.");

        var product = await _service.GetByIdAsync(id);
        return product is null
            ? NotFound($"Product with ID {id} not found.")
            : Ok(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] CreateProductDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return BadRequest("Product name cannot be empty.");
        
        if (dto.Price <= 0)
            return BadRequest("Product price must be greater than zero.");

        if (dto.CategoryId <= 0)
            return BadRequest("Invalid category ID.");

        var generatedId = await _service.AddAsync(dto);
        return CreatedAtAction("GetById", new { id = generatedId }, dto);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateProductDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Product ID mismatch.");

        var existingProduct = await _service.GetByIdAsync(id);
        if (existingProduct == null)
            return NotFound($"Product with ID {id} not found.");

        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        if (id <= 0) return BadRequest("Invalid product ID.");

        var existingProduct = await _service.GetByIdAsync(id);
        if (existingProduct == null)
            return NotFound($"Product with ID {id} not found.");

        await _service.DeleteAsync(id);
        return NoContent();
    }
}