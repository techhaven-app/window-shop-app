using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Shared.DTOs.Auth;

namespace TechHaven.Application.Features.Auth.Signup;
[ExcludeFromCodeCoverage] //Chỉ lưu trữ dữ liệu - không cần test
public record SignupCommand(
  string UserFullName,
  string Email,
  string UserName,
  string Password,
// string ConfirmPassword,
  int RoleId
) : ICommand<SignupResponseDto>;