using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Shared.DTOs.Auth;

namespace TechHaven.Application.Features.Auth.VerifyOtp;

[ExcludeFromCodeCoverage] // Dùng để test riêng, không cần coverage
public record VerifyOtpCommand(string OtpSessionId, string OtpCode) : ICommand<OtpVerifyResponseDto>;