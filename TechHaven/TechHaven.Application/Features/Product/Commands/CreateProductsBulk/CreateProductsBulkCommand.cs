using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Products;

namespace TechHaven.Application.Features.Product.Commands.CreateProductsBulk;
[ExcludeFromCodeCoverage] // Loại biên lúc test vì chỉ có vai trò chứa dữ liệu
public class CreateProductsBulkCommand : ICommand<Result<ProductBulkCreateResponseDto>>
{
  public List<ProductUpsertRequest> Products { get; init; } = new();
  public bool SkipDuplicates { get; init; } = true;
  public bool ValidateBeforeInsert { get; init; } = true;
}