using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Productify.Api.Controllers;
using Productify.Application.Contracts;
using Productify.Application.DTOs;
using Productify.Domain.Entities;

namespace Productify.Tests.Controllers;

public class CategoryControllerTests
{
    private readonly Mock<IGenericService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>> _mockService;
    private readonly CategoryController _controller;
    
    public CategoryControllerTests()
    {
        _mockService = new Mock<IGenericService<Category, CategoryDto, CreateCategoryDto, UpdateCategoryDto>>();
        _controller = new CategoryController(_mockService.Object);
    }
    
    #region GetAllAsync
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnOk_WhenDataExists()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllAsync(0, 10))
            .ReturnsAsync(new List<CategoryDto> { new(1, "Electronics") });

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<CategoryDto>>()
            .Which.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoDataExists()
    {
        // Arrange
        _mockService.Setup(s => s.GetAllAsync(0, 10))
            .ReturnsAsync(new List<CategoryDto>());

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<CategoryDto>>()
            .Which.Should().BeEmpty();
    }
    
    #endregion
    
    #region GetByIdAsync
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnOk_WhenCategoryExists()
    {
        // Arrange
        var categoryDto = new CategoryDto(1, "Electronics");
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(categoryDto);

        // Act
        var result = await _controller.GetByIdAsync(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(categoryDto);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((CategoryDto?)null);

        // Act
        var result = await _controller.GetByIdAsync(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
    
    #endregion
    
    #region CreateAsync
    
    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenCategoryIsAdded()
    {
        // Arrange
        var createDto = new CreateCategoryDto ("New Category");
        _mockService.Setup(s => s.AddAsync(It.IsAny<CreateCategoryDto>()))
            .ReturnsAsync(1); // Simula o ID gerado

        // Act
        var result = await _controller.CreateAsync(createDto);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.ActionName.Should().Be("GetById");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        // Arrange
        var createDto = new CreateCategoryDto ("");

        // Act
        var result = await _controller.CreateAsync(createDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Category name cannot be empty.");
    }
    
    #endregion
    
    #region UpdateAsync
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var updateDto = new UpdateCategoryDto(1, "Updated Category");
        _mockService.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(new CategoryDto(1, "Old Category"));
    
        _mockService.Setup(s => s.UpdateAsync(It.IsAny<UpdateCategoryDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateAsync(1, updateDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnBadRequest_WhenIdMismatch()
    {
        // Arrange
        var updateDto = new UpdateCategoryDto(1, "Updated Category");

        // Act
        var result = await _controller.UpdateAsync(2, updateDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("ID mismatch.");
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenCategoryIsDeleted()
    {
        // Arrange
        var existingCategory = new CategoryDto(1, "Existing Category");
        _mockService.Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(existingCategory);  // Retorna uma categoria válida

        _mockService.Setup(s => s.DeleteAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask); // Exclusão bem-sucedida

        // Act
        var result = await _controller.DeleteAsync(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((CategoryDto?)null); // Simula item inexistente.

        // Act
        var result = await _controller.DeleteAsync(999);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>()
            .Which.Value.Should().Be("Category not found.");
    }

    #endregion
}