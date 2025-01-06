using FluentValidation;
using Productify.Domain.Entities;

namespace Productify.Application.Validations;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("The name is required.")
            .MaximumLength(100)
            .WithMessage("The name must not exceed 100 characters.");
    }
}