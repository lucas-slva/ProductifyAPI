namespace Productify.Application.DTOs;

public record CategoryDto(int Id, string Name);
public record CreateCategoryDto(string Name);
public record UpdateCategoryDto(int Id, string Name);