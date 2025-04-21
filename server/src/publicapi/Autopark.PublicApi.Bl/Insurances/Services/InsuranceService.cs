using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.Insurances.Errors;
using Autopark.PublicApi.Bl.Insurances.Services.Interfaces;
using Autopark.PublicApi.Models.Insurances;
using Autopark.PublicApi.Shared.Insurances.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.Insurances.Services;

[ServiceAsInterfaces]
public class InsuranceService(
    IEditDtoRepository<Insurance, InsuranceRequest> insuranceEditDtoRepository,
    IDtoRepository<Insurance, InsuranceResponse> insuranceDtoRepository) : IInsuranceService
{
    public async Task<Result<QueryResultDto<InsuranceResponse>>> GetAllAsync(QueryFilter<InsuranceFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await insuranceDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<InsuranceResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var insuranceDto = await insuranceDtoRepository.GetDtoAsync(id, cancellationToken);

        if (insuranceDto is null)
        {
            return new InsuranceNotFoundError(id);
        }

        return insuranceDto;
    }

    public async Task<Result<InsuranceResponse>> CreateInsuranceAsync(InsuranceRequest insuranceRequest, CancellationToken cancellationToken = default)
    {
        var result = await EnsureInsuranceBySeriesAndNumberNotExistAsync(insuranceRequest.Series, insuranceRequest.Number, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        try
        {
            var id = await insuranceEditDtoRepository.SaveDtoAsync(insuranceRequest, cancellationToken: cancellationToken);

            return (await insuranceDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Insurance.Create");
        }
    }

    public async Task<Result<InsuranceResponse>> UpdateInsuranceAsync(Guid id, InsuranceRequest insuranceRequest, CancellationToken cancellationToken = default)
    {
        var insuranceDto = await insuranceDtoRepository.GetDtoAsync(id, cancellationToken);
        if (insuranceDto is null)
        {
            return new InsuranceNotFoundError(id);
        }

        if (insuranceDto.Series != insuranceRequest.Series || insuranceDto.Number != insuranceRequest.Number)
        {
            var result = await EnsureInsuranceBySeriesAndNumberNotExistAsync(insuranceRequest.Series, insuranceRequest.Number, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }    
        }

        try
        {
            var updatedId = await insuranceEditDtoRepository.SaveDtoAsync(insuranceRequest, cancellationToken: cancellationToken);

            return (await insuranceDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("Insurance.Update");
        }
    }

    public async Task<Result> DeleteInsuranceAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var insurance = await insuranceDtoRepository.GetDtoAsync(id, cancellationToken);

        if (insurance is null)
        {
            return new InsuranceNotFoundError(id);
        }

        try
        {
            await insuranceDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("Insurance.Delete");
        }

        return Result.Ok();
    }

    private async Task<Result> EnsureInsuranceBySeriesAndNumberNotExistAsync(string series, string number, CancellationToken cancellationToken = default)
    {
        var insuranceExist = await insuranceEditDtoRepository.AnyByFilterAsync(
            new InsuranceFilterDto()
            {
                Series = series,
                Number = number,
            }, cancellationToken);

        if (insuranceExist)
        {
            return new InsuranceDuplicationError($"{series}{number}");
        }

        return Result.Ok();
    }
}
