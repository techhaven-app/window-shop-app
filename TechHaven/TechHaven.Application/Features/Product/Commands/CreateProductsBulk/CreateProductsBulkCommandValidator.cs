// TechHaven.Application/Features/Product/Commands/CreateProductsBulk/CreateProductsBulkCommandValidator.cs
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TechHaven.Application.Features.Product.Commands.CreateProductsBulk;

[ExcludeFromCodeCoverage] // Loại biên lúc test vì logic validate này đã được test cho file CreateProduct/CreateProductCommandValidator.cs
public class CreateProductsBulkCommandValidator : AbstractValidator<CreateProductsBulkCommand>
{
  public CreateProductsBulkCommandValidator()
  {
    RuleFor(x => x.Products)
      .NotEmpty().WithMessage("Products list cannot be empty")
      .Must(products => products != null && products.Count > 0)
      .WithMessage("At least one product is required");

    RuleFor(x => x.Products.Count)
      .LessThanOrEqualTo(1000)
      .WithMessage("Maximum 1000 products can be imported at once");
  }
}