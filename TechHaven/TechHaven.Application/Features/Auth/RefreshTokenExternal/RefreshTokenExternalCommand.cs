using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Shared.DTOs.Auth;

namespace TechHaven.Application.Features.Auth.RefreshTokenExternal;

[ExcludeFromCodeCoverage] // Dùng để loại trừ test riêng, không cần coverage
public record RefreshTokenExternalCommand(
    string RefreshToken,
    string EncryptedDbConfig)
  : ICommand<RefreshTokenResponseDto>;