using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Shared.DTOs.Auth;

namespace TechHaven.Application.Features.Auth.Login;
[ExcludeFromCodeCoverage]
public record LoginCommand(string UserName, string Password) : ICommand<LoginResponseDto>;