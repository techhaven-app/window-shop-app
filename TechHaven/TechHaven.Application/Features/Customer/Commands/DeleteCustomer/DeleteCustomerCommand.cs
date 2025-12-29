using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;

namespace TechHaven.Application.Features.Customer.Commands.DeleteCustomer;
[ExcludeFromCodeCoverage] //Đây chỉ là Command chứa dữ liệu - ko cần test file này

public record DeleteCustomerCommand(int CustomerId) : ICommand<Result>;