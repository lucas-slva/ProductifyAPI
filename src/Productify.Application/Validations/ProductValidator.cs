using FluentValidation;
using Productify.Domain.Entities;

namespace Productify.Api.Validations;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("The product name is required")
            .MaximumLength(100)
            .WithMessage("The name must not exceed 100 characters.");
        
        RuleFor(p => p.Price)
            .NotEmpty()
            .WithMessage("The product price is required")
            .GreaterThan(0)
            .WithMessage("Product price must be greater than 0");
    }
}