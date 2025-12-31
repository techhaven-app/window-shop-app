using System.Diagnostics.CodeAnalysis;
using TechHaven.Application.Interfaces;
using TechHaven.Domain.Common;
using TechHaven.Shared.DTOs.Reports;

namespace TechHaven.Application.Features.Reports.Queries.GetDashboardSummary;
[ExcludeFromCodeCoverage]
public record GetDashboardSummaryQuery() : IQuery<Result<DashboardSummaryDto>>;