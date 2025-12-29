using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Products;

namespace TechHaven.Application.Features.Product.Queries.GetProductById;
[ExcludeFromCodeCoverage] // Loại biên này lúc test vì chỉ vai trò truy xuất cơ bản

public record GetProductByIdQuery(int ProductId) : IQuery<Result<ProductDto>>;