using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.TechnicalPassports.Services.Interfaces;
using Autopark.PublicApi.Shared.TechnicalPassports.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class TechnicalPassportController(ITechnicalPassportService technicalPassportService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<TechnicalPassportFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await technicalPassportService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{technicalPassportId:guid}")]
    public async Task<IResult> GetTechnicalPassportByIdAsync(Guid technicalPassportId, CancellationToken cancellationToken = default)
    {
        var result = await technicalPassportService.GetByIdAsync(technicalPassportId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> CreateTechnicalPassportAsync(
        [FromBody] TechnicalPassportRequest createTechnicalPassportRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await technicalPassportService.CreateTechnicalPassportAsync(createTechnicalPassportRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{technicalPassportId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> UpdateTechnicalPassportAsync(
        Guid technicalPassportId,
        [FromBody] TechnicalPassportRequest updateTechnicalPassportRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await technicalPassportService.UpdateTechnicalPassportAsync(technicalPassportId, updateTechnicalPassportRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{technicalPassportId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.DocumentSpecialist}")]
    public async Task<IResult> DeleteTechnicalPassportAsync(Guid technicalPassportId, CancellationToken cancellationToken = default)
    {
        var result = await technicalPassportService.DeleteTechnicalPassportAsync(technicalPassportId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
