using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.PublicApi.Shared.TechnicalPassports.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.TechnicalPassports.Services.Interfaces;

public interface ITechnicalPassportService
{
    Task<Result<QueryResultDto<TechnicalPassportResponse>>> GetAllAsync(QueryFilter<TechnicalPassportFilterDto> filter, CancellationToken cancellationToken = default);

    Task<Result<TechnicalPassportResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<TechnicalPassportResponse>> CreateTechnicalPassportAsync(
        TechnicalPassportRequest technicalPassportRequest,
        CancellationToken cancellationToken = default);

    Task<Result<TechnicalPassportResponse>> UpdateTechnicalPassportAsync(
        Guid id,
        TechnicalPassportRequest technicalPassportRequest,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteTechnicalPassportAsync(Guid id, CancellationToken cancellationToken = default);
}
