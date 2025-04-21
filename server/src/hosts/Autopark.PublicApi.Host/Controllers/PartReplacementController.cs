using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.PartReplacements.Services.Interfaces;
using Autopark.PublicApi.Shared.PartReplacements.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class PartReplacementController(IPartReplacementService partReplacementService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<PartReplacementFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await partReplacementService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{partReplacementId:guid}")]
    public async Task<IResult> GetPartReplacementByIdAsync(Guid partReplacementId, CancellationToken cancellationToken = default)
    {
        var result = await partReplacementService.GetByIdAsync(partReplacementId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Technician}")]
    public async Task<IResult> CreatePartReplacementAsync(
        [FromBody] PartReplacementRequest createPartReplacementRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await partReplacementService.CreatePartReplacementAsync(createPartReplacementRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{partReplacementId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> UpdatePartReplacementAsync(
        Guid partReplacementId,
        [FromBody] PartReplacementRequest updatePartReplacementRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await partReplacementService.UpdatePartReplacementAsync(partReplacementId, updatePartReplacementRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{partReplacementId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}")]
    public async Task<IResult> DeletePartReplacementAsync(Guid partReplacementId, CancellationToken cancellationToken = default)
    {
        var result = await partReplacementService.DeletePartReplacementAsync(partReplacementId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
