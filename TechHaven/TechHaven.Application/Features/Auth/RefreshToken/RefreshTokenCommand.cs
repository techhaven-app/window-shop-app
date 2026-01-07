using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Shared.DTOs.Auth;

namespace TechHaven.Application.Features.Auth.RefreshToken;

[ExcludeFromCodeCoverage] // Dùng để loại trừ test riêng, không cần coverage
public record RefreshTokenCommand(string RefreshToken)
  : ICommand<RefreshTokenResponseDto>;