using FluentValidation;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace TechHaven.Application.Features.Auth.LoginExternal;
[ExcludeFromCodeCoverage] // Đã test ở file LoginCommandValidator.cs rồi - giống logic nhau
public class LoginExternalCommandValidator : AbstractValidator<LoginExternalCommand>
{
  public LoginExternalCommandValidator()
  {
    RuleFor(x => x.UserName)
        .NotEmpty().WithMessage("Username is required.")
        .MaximumLength(50).WithMessage("Username must be at most 50 characters long.");

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
  }
}