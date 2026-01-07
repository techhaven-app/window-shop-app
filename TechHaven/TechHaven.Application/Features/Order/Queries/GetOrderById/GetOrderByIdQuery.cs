using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Orders;

namespace TechHaven.Application.Features.Order.Queries.GetOrderById;
[ExcludeFromCodeCoverage] 

/// <summary>
/// Get Order by Id Query
/// </summary>
/// <param name="OrderId"></param>
public record GetOrderByIdQuery(int OrderId) : IQuery<Result<OrderDto>>;