using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Auth;

namespace TechHaven.Application.Features.Auth.Queries.GetCurrentUser;
[ExcludeFromCodeCoverage] //Get Query logic cơ bản - ưu tiên không test để tối ưu thời gian

/// <summary>
/// Query to get current user information from access token
/// </summary>
public record GetCurrentUserQuery : IQuery<Result<UserInfoDto>>;