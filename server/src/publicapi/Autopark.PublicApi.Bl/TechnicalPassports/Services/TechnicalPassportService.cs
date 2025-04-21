using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.TechnicalPassports.Errors;
using Autopark.PublicApi.Bl.TechnicalPassports.Services.Interfaces;
using Autopark.PublicApi.Models.TechnicalPassports;
using Autopark.PublicApi.Shared.TechnicalPassports.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.TechnicalPassports.Services;

[ServiceAsInterfaces]
public class TechnicalPassportService(
    IEditDtoRepository<TechnicalPassport, TechnicalPassportRequest> technicalPassportEditDtoRepository,
    IDtoRepository<TechnicalPassport, TechnicalPassportResponse> technicalPassportDtoRepository) : ITechnicalPassportService
{
    public async Task<Result<QueryResultDto<TechnicalPassportResponse>>> GetAllAsync(QueryFilter<TechnicalPassportFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await technicalPassportDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<TechnicalPassportResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var technicalPassportDto = await technicalPassportDtoRepository.GetDtoAsync(id, cancellationToken);

        if (technicalPassportDto is null)
        {
            return new TechnicalPassportNotFoundError(id);
        }

        return technicalPassportDto;
    }

    public async Task<Result<TechnicalPassportResponse>> CreateTechnicalPassportAsync(TechnicalPassportRequest technicalPassportRequest, CancellationToken cancellationToken = default)
    {
        var result = await EnsureTechnicalPassportForVehicleNotExist(technicalPassportRequest.VehicleId, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        result = await EnsureTechnicalPassportForNumberNotExist(technicalPassportRequest.Number, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        result = await EnsureTechnicalPassportForVINNotExist(technicalPassportRequest.VIN, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        result = await EnsureTechnicalPassportForLicensePlateNotExist(technicalPassportRequest.LicensePlate, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        try
        {
            var id = await technicalPassportEditDtoRepository.SaveDtoAsync(technicalPassportRequest, cancellationToken: cancellationToken);

            return (await technicalPassportDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("TechnicalPassport.Create");
        }
    }

    public async Task<Result<TechnicalPassportResponse>> UpdateTechnicalPassportAsync(Guid id, TechnicalPassportRequest technicalPassportRequest, CancellationToken cancellationToken = default)
    {
        var technicalPassportDto = await technicalPassportDtoRepository.GetDtoAsync(id, cancellationToken);
        if (technicalPassportDto is null)
        {
            return new TechnicalPassportNotFoundError(id);
        }

        if (technicalPassportRequest.Number != technicalPassportDto.Number)
        {
            var result = await EnsureTechnicalPassportForNumberNotExist(technicalPassportRequest.Number, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }
        }

        if (technicalPassportRequest.VIN != technicalPassportDto.VIN)
        {
            var result = await EnsureTechnicalPassportForVINNotExist(technicalPassportRequest.VIN, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }
        }

        if (technicalPassportRequest.LicensePlate != technicalPassportDto.LicensePlate)
        {
            var result = await EnsureTechnicalPassportForLicensePlateNotExist(technicalPassportRequest.LicensePlate, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }
        }

        if (technicalPassportRequest.VehicleId != technicalPassportDto.VehicleId)
        {
            var result = await EnsureTechnicalPassportForVehicleNotExist(technicalPassportRequest.VehicleId, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }
        }

        try
        {
            var updatedId = await technicalPassportEditDtoRepository.SaveDtoAsync(technicalPassportRequest, cancellationToken: cancellationToken);

            return (await technicalPassportDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("TechnicalPassport.Update");
        }
    }

    public async Task<Result> DeleteTechnicalPassportAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var technicalPassport = await technicalPassportDtoRepository.GetDtoAsync(id, cancellationToken);
        if (technicalPassport is null)
        {
            return new TechnicalPassportNotFoundError(id);
        }

        try
        {
            await technicalPassportDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("TechnicalPassport.Delete");
        }

        return Result.Ok();
    }

    private async Task<Result> EnsureTechnicalPassportForVehicleNotExist(Guid vehicleId, CancellationToken cancellationToken)
    {
        var technicalPassportForVehicleExist = await technicalPassportDtoRepository.AnyByFilterAsync(
            new TechnicalPassportFilterDto()
            {
                VehicleId = vehicleId 
            }, cancellationToken);

        if (technicalPassportForVehicleExist)
        {
            return new TechnicalPassportDuplicationError(vehicleId);
        }

        return Result.Ok();
    }

    private async Task<Result> EnsureTechnicalPassportForNumberNotExist(string number, CancellationToken cancellationToken)
    {
        var technicalPassportExist = await technicalPassportDtoRepository.AnyByFilterAsync(
            new TechnicalPassportFilterDto()
            {
                Number = number 
            }, cancellationToken);

        if (technicalPassportExist)
        {
            return new TechnicalPassportDuplicationError(number);
        }

        return Result.Ok();
    }

    private async Task<Result> EnsureTechnicalPassportForVINNotExist(string vin, CancellationToken cancellationToken)
    {
        var technicalPassportExist = await technicalPassportDtoRepository.AnyByFilterAsync(
            new TechnicalPassportFilterDto()
            {
                VIN = vin 
            }, cancellationToken);

        if (technicalPassportExist)
        {
            return new TechnicalPassportDuplicationError(vin);
        }

        return Result.Ok();
    }

    private async Task<Result> EnsureTechnicalPassportForLicensePlateNotExist(string licensePlate, CancellationToken cancellationToken)
    {
        var technicalPassportExist = await technicalPassportDtoRepository.AnyByFilterAsync(
            new TechnicalPassportFilterDto()
            {
                LicensePlate = licensePlate 
            }, cancellationToken);

        if (technicalPassportExist)
        {
            return new TechnicalPassportDuplicationError(licensePlate);
        }

        return Result.Ok();
    }
}
