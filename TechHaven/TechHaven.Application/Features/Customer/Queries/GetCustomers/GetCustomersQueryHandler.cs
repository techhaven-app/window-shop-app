using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Domain.Interfaces;
using TechHaven.Shared.DTOs.Common;
using TechHaven.Shared.DTOs.Customers;

namespace TechHaven.Application.Features.Customer.Queries.GetCustomers;
[ExcludeFromCodeCoverage] // Query Handler cơ bản - không cần test riêng

public class GetCustomersQueryHandler : IQueryHandler<GetCustomersQuery, Result<PagingResponse<CustomerDto>>>
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IMapper _mapper;

  public GetCustomersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
  {
    _unitOfWork = unitOfWork;
    _mapper = mapper;
  }

  public async Task<Result<PagingResponse<CustomerDto>>> Handle(
    GetCustomersQuery request,
    CancellationToken cancellationToken)
  {
    try
    {
      var (customers, totalCount) = await _unitOfWork.Customers.SearchWithPaginationAsync(
        request.criteria,
        cancellationToken
      );

      var CustomersDto = _mapper.Map<IReadOnlyList<CustomerDto>>(customers);

      var response = new PagingResponse<CustomerDto>
      {
        Items = CustomersDto,
        PageNumber = request.criteria.PageNumber,
        PageSize = request.criteria.PageSize,
        TotalCount = totalCount
      };

      return Result<PagingResponse<CustomerDto>>.Success(response);
    }
    catch (Exception ex)
    {
      return Result<PagingResponse<CustomerDto>>.Failure(
        $"Failed to get customers: {ex.Message}",
        ErrorType.InternalError);
    }
  }
}