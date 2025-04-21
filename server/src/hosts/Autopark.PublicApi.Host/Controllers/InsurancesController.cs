using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.Insurances.Services.Interfaces;
using Autopark.PublicApi.Shared.Insurances.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class InsuranceController(IInsuranceService insuranceService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<InsuranceFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await insuranceService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{insuranceId:guid}")]
    public async Task<IResult> GetInsuranceByIdAsync(Guid insuranceId, CancellationToken cancellationToken = default)
    {
        var result = await insuranceService.GetByIdAsync(insuranceId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> CreateInsuranceAsync(
        [FromBody] InsuranceRequest createInsuranceRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await insuranceService.CreateInsuranceAsync(createInsuranceRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{insuranceId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> UpdateInsuranceAsync(
        Guid insuranceId,
        [FromBody] InsuranceRequest updateInsuranceRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await insuranceService.UpdateInsuranceAsync(insuranceId, updateInsuranceRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{insuranceId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> DeleteInsuranceAsync(Guid insuranceId, CancellationToken cancellationToken = default)
    {
        var result = await insuranceService.DeleteInsuranceAsync(insuranceId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
