using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Auth;

namespace TechHaven.Application.Features.Auth.Activate;
[ExcludeFromCodeCoverage] //POST sửa đổi trạng thái cơ bản - ưu tiên không test để tối ưu thời gian

/// <summary>
/// Command for activating a user account.
/// </summary>
public record ActivateCommand(int UserId, string Key) : ICommand<ActivateResponseDto>;