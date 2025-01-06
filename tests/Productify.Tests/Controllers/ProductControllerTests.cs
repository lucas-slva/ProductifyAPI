using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Productify.Api.Controllers;
using Productify.Application.Contracts;
using Productify.Application.DTOs;
using Productify.Domain.Entities;

namespace Productify.Tests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IGenericService<Product, ProductDto, CreateProductDto, UpdateProductDto>> _mockService;
    private readonly ProductController _controller;
    
    public ProductControllerTests()
    {
        _mockService = new Mock<IGenericService<Product, ProductDto, CreateProductDto, UpdateProductDto>>();
        _controller = new ProductController(_mockService.Object);
    }
    
    #region GetRangeAsync

    [Fact]
    public async Task GetRangeAsync_ShouldReturnOk_WhenValidInput()
    {
        _mockService.Setup(s => s.GetAllAsync(0, 10))
            .ReturnsAsync(new List<ProductDto> { new(1, "Laptop", 1500, "Electronics") });

        var result = await _controller.GetRangeAsync();

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProductDto>>();
    }

    [Fact]
    public async Task GetRangeAsync_ShouldReturnBadRequest_WhenInvalidInput()
    {
        var result = await _controller.GetRangeAsync(-1, 0);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Invalid pagination parameters.");
    }

    #endregion

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOk_WhenProductExists()
    {
        var productDto = new ProductDto(1, "Laptop", 1500, "Electronics");
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(productDto);

        var result = await _controller.GetByIdAsync(1);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(productDto);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductDto?)null);

        var result = await _controller.GetByIdAsync(999);

        result.Should().BeOfType<NotFoundObjectResult>()
            .Which.Value.Should().Be("Product with ID 999 not found.");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBadRequest_WhenInvalidId()
    {
        var result = await _controller.GetByIdAsync(-1);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Invalid product ID.");
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_ShouldReturnCreated_WhenProductIsAdded()
    {
        var createDto = new CreateProductDto("Laptop", 1500, 1);
        _mockService.Setup(s => s.AddAsync(It.IsAny<CreateProductDto>()))
            .ReturnsAsync(1); // Simula ID gerado

        var result = await _controller.CreateAsync(createDto);

        result.Should().BeOfType<CreatedAtActionResult>()
            .Which.ActionName.Should().Be("GetById");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenNameIsEmpty()
    {
        var createDto = new CreateProductDto("", 1500, 1);

        var result = await _controller.CreateAsync(createDto);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Product name cannot be empty.");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenPriceIsInvalid()
    {
        var createDto = new CreateProductDto("Laptop", -100, 1);

        var result = await _controller.CreateAsync(createDto);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Product price must be greater than zero.");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnBadRequest_WhenCategoryIdIsInvalid()
    {
        var createDto = new CreateProductDto("Laptop", 1500, 0);

        var result = await _controller.CreateAsync(createDto);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Invalid category ID.");
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ShouldReturnNoContent_WhenUpdateIsSuccessful()
    {
        var updateDto = new UpdateProductDto(1, "Updated Laptop", 1600, 1);
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new ProductDto(1, "Laptop", 1500, "Electronics"));
        _mockService.Setup(s => s.UpdateAsync(updateDto)).Returns(Task.CompletedTask);

        var result = await _controller.UpdateAsync(1, updateDto);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnBadRequest_WhenIdMismatch()
    {
        var updateDto = new UpdateProductDto(1, "Updated Laptop", 1600, 1);

        var result = await _controller.UpdateAsync(2, updateDto);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Product ID mismatch.");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        var updateDto = new UpdateProductDto(1, "Updated Laptop", 1600, 1);
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((ProductDto?)null);

        var result = await _controller.UpdateAsync(1, updateDto);

        result.Should().BeOfType<NotFoundObjectResult>()
            .Which.Value.Should().Be("Product with ID 1 not found.");
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent_WhenProductIsDeleted()
    {
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new ProductDto(1, "Laptop", 1500, "Electronics"));
        _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteAsync(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductDto?)null);

        var result = await _controller.DeleteAsync(999);

        result.Should().BeOfType<NotFoundObjectResult>()
            .Which.Value.Should().Be("Product with ID 999 not found.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnBadRequest_WhenInvalidId()
    {
        var result = await _controller.DeleteAsync(-1);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Invalid product ID.");
    }

    #endregion
}