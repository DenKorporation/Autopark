using Asp.Versioning;
using Autopark.Common.Constants;
using Autopark.Common.Web.Extensions;
using Autopark.Common.Web.Security;
using Autopark.Dal.Core.Filters;
using Autopark.PublicApi.Bl.Parts.Services.Interfaces;
using Autopark.PublicApi.Shared.Parts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Autopark.PublicApi.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{apiVersion:apiVersion}/[controller]/[action]")]
public class PartController(IPartService partService) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> PostQueryAsync(
        [FromBody] QueryFilter<PartFilterDto> filterRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await partService.GetAllAsync(filterRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpGet("{partId:guid}")]
    public async Task<IResult> GetPartByIdAsync(Guid partId, CancellationToken cancellationToken = default)
    {
        var result = await partService.GetByIdAsync(partId, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpPost]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Technician}")]
    public async Task<IResult> CreatePartAsync(
        [FromBody] PartRequest createPartRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await partService.CreatePartAsync(createPartRequest, cancellationToken);

        return result.ToAspResult(value => Results.Created(string.Empty, value));
    }

    [HttpPut("{partId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Technician}")]
    public async Task<IResult> UpdatePartAsync(
        Guid partId,
        [FromBody] PartRequest updatePartRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await partService.UpdatePartAsync(partId, updatePartRequest, cancellationToken);

        return result.ToAspResult(Results.Ok);
    }

    [HttpDelete("{partId:guid}")]
    [Permission(Roles = $"{Roles.Administrator}, {Roles.Technician}")]
    public async Task<IResult> DeletePartAsync(Guid partId, CancellationToken cancellationToken = default)
    {
        var result = await partService.DeletePartAsync(partId, cancellationToken);

        return result.ToAspResult(Results.NoContent);
    }
}
