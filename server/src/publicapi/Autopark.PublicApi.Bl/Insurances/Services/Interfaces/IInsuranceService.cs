using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.Insurances.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Insurances.Services.Interfaces;

public interface IInsuranceService
{
    Task<Result<QueryResultDto<InsuranceResponse>>> GetAllAsync(QueryFilter<InsuranceFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<InsuranceResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<InsuranceResponse>> CreateInsuranceAsync(
        InsuranceRequest insuranceRequest,
        CancellationToken cancellationToken = default);

    Task<Result<InsuranceResponse>> UpdateInsuranceAsync(
        Guid id,
        InsuranceRequest insuranceRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteInsuranceAsync(Guid id, CancellationToken cancellationToken = default);
}
