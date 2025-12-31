using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace TechHaven.Application.Features.Auth.RefreshToken;

[ExcludeFromCodeCoverage] // Logic này đã được test trong RefresTokenExternalCommandValidator.cs
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
  public RefreshTokenCommandValidator()
  {
    RuleFor(x => x.RefreshToken)
      .NotEmpty().WithMessage("Refresh token is required")
      .MinimumLength(20).WithMessage("Invalid refresh token format");
  }
}