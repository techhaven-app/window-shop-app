using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;

namespace TechHaven.Application.Features.Product.Commands.DeleteProduct;
[ExcludeFromCodeCoverage] // Loại biên lúc test vì chỉ có vai trò chứa dữ liệu
public record DeleteProductCommand(int ProductId) : ICommand<Result>;