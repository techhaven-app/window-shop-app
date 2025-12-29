using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Customers;

namespace TechHaven.Application.Features.Customer.Commands.CreateCustomer;
[ExcludeFromCodeCoverage] //Đây chỉ là Command chứa dữ liệu - ko cần test file này

public record CreateCustomerCommand() : ICommand<Result<CustomerDto>>
{
  public int CustomerId { get; set; }
  public string CustomerName { get; set; } = string.Empty;
  public string PhoneNumber { get; set; } = string.Empty;
  public string? Email { get; set; }
  public string? Address { get; set; }
  public CustomerType Type { get; set; }
  public string? Note { get; set; }
}