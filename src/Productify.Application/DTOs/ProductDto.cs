namespace Productify.Application.DTOs;

public record ProductDto(int Id, string Name, decimal Price, string CategoryName);
public record CreateProductDto(string Name, decimal Price, int CategoryId);
public record UpdateProductDto(int Id, string Name, decimal Price, int CategoryId);