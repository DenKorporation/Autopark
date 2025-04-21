using Autopark.Common.Attributes;
using Autopark.Common.Bl.Errors;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.Repositories;
using Autopark.PublicApi.Bl.FuelTypes.Errors;
using Autopark.PublicApi.Bl.FuelTypes.Services.Interfaces;
using Autopark.PublicApi.Models.FuelTypes;
using Autopark.PublicApi.Shared.FuelTypes.Dto;
using FluentResults;

namespace Autopark.PublicApi.Bl.FuelTypes.Services;

[ServiceAsInterfaces]
public class FuelTypeService(
    IEditDtoRepository<FuelType, FuelTypeRequest> fuelTypeEditDtoRepository,
    IDtoRepository<FuelType, FuelTypeResponse> fuelTypeDtoRepository) : IFuelTypeService
{
    public async Task<Result<QueryResultDto<FuelTypeResponse>>> GetAllAsync(QueryFilter<FuelTypeFilterDto> filter, CancellationToken cancellationToken = default)
    {
        return await fuelTypeDtoRepository.QueryDtoAsync(filter, cancellationToken);
    }

    public async Task<Result<FuelTypeResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var fuelTypeDto = await fuelTypeDtoRepository.GetDtoAsync(id, cancellationToken);

        if (fuelTypeDto is null)
        {
            return new FuelTypeNotFoundError(id);
        }

        return fuelTypeDto;
    }

    public async Task<Result<FuelTypeResponse>> CreateFuelTypeAsync(FuelTypeRequest fuelTypeRequest, CancellationToken cancellationToken = default)
    {
        var result = await EnsureFuelTypeByNameNotExistAsync(fuelTypeRequest.Name, cancellationToken);
        if (result.IsFailed)
        {
            return result;
        }

        try
        {
            var id = await fuelTypeEditDtoRepository.SaveDtoAsync(fuelTypeRequest, cancellationToken: cancellationToken);

            return (await fuelTypeDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("FuelType.Create");
        }
    }

    public async Task<Result<FuelTypeResponse>> UpdateFuelTypeAsync(Guid id, FuelTypeRequest fuelTypeRequest, CancellationToken cancellationToken = default)
    {
        var fuelTypeDto = await fuelTypeDtoRepository.GetDtoAsync(id, cancellationToken);
        if (fuelTypeDto is null)
        {
            return new FuelTypeNotFoundError(id);
        }

        if (fuelTypeDto.Name != fuelTypeRequest.Name)
        {
            var result = await EnsureFuelTypeByNameNotExistAsync(fuelTypeRequest.Name, cancellationToken);
            if (result.IsFailed)
            {
                return result;
            }    
        }

        try
        {
            var updatedId = await fuelTypeEditDtoRepository.SaveDtoAsync(fuelTypeRequest, cancellationToken: cancellationToken);

            return (await fuelTypeDtoRepository.GetDtoAsync(id, cancellationToken))!;
        }
        catch (Exception)
        {
            return new InternalServerError("FuelType.Update");
        }
    }

    public async Task<Result> DeleteFuelTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var fuelType = await fuelTypeDtoRepository.GetDtoAsync(id, cancellationToken);

        if (fuelType is null)
        {
            return new FuelTypeNotFoundError(id);
        }

        try
        {
            await fuelTypeDtoRepository.DeleteByIdAsync(id);
        }
        catch (Exception)
        {
            return new InternalServerError("FuelType.Delete");
        }

        return Result.Ok();
    }

    private async Task<Result> EnsureFuelTypeByNameNotExistAsync(string name, CancellationToken cancellationToken = default)
    {
        var fuelTypeExist = await fuelTypeEditDtoRepository.AnyByFilterAsync(
            new FuelTypeFilterDto()
            {
                Name = name,
            }, cancellationToken);

        if (fuelTypeExist)
        {
            return new FuelTypeDuplicationError(name);
        }

        return Result.Ok();
    }
}
