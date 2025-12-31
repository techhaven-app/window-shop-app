using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Auth;

namespace TechHaven.Application.Features.Auth.Queries.IsActive;
[ExcludeFromCodeCoverage] //Get Query logic cơ bản - ưu tiên không test để tối ưu thời gian
public record IsActiveQuery(int UserId) : IQuery<IsActiveResponseDto>;