using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Domain.SearchCriteria;
using TechHaven.Shared.DTOs.Common;
using TechHaven.Shared.DTOs.Products;

namespace TechHaven.Application.Features.Product.Queries.GetProducts;
[ExcludeFromCodeCoverage] // Loại biên này lúc test vì chỉ vai trò truy xuất cơ bản - không có logic tính toán phức tạp
public record GetProductsQuery(ProductSearchCriteria criteria) : IQuery<Result<PagingResponse<ProductDto>>>;