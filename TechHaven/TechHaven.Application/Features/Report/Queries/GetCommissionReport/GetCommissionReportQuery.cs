using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Reports;

namespace TechHaven.Application.Features.Reports.Queries.GetCommissionReport;
[ExcludeFromCodeCoverage] //Query logic đơn gian -> Không test để tối ưu thời gian
public record GetCommissionReportQuery(
  int Month,
  int Year
) : IQuery<Result<List<CommissionReportDto>>>;