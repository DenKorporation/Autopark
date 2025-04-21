using Autopark.Common.Domain;
using Autopark.Common.Mapping;
using Autopark.Common.Security;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.QueryTransformers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autopark.Dal.Core.Repositories;

public class DefaultDtoRepository<TEntity> : DefaultRepository<TEntity>, IDtoRepository<TEntity>
    where TEntity : class, IEntityBase, new()
{
    private readonly IServiceProvider serviceProvider;

    public DefaultDtoRepository(
        DbContext dbContext,
        ILogger<DefaultDtoRepository<TEntity>> logger,
        IUserInfoProvider userInfoProvider,
        IServiceProvider serviceProvider,
        IQueryTransformer<TEntity> queryTransformer)
        : base(dbContext, logger, userInfoProvider, queryTransformer)
    {
        this.serviceProvider = serviceProvider;
    }

    public virtual async Task<IQueryResultDto<TDto>> QueryDtoAsync<TFilter, TDto>(
        QueryBase<TFilter, TDto> queryDto,
        CancellationToken ct = default)
        where TFilter : class, new()
        where TDto : class, new()
    {
        var mapper = serviceProvider.GetRequiredService<IMapper<TEntity, TDto>>();

        var filter = mapper.Map<QueryBase<TFilter, TDto>, QueryBase<TFilter, TEntity>>(queryDto);

        var queryResult = await QueryAsync(filter, ct);

        var result = await MapResult(mapper, queryResult, ct);

        return result;
    }

    /// <inheritdoc />
    public virtual async Task<TDto> GetDtoAsync<TDto>(Guid id, CancellationToken ct = default)
        where TDto : class, new()
    {
        var ent = await GetEntityAsync<TEntity>(id, ct);
        var mapper = serviceProvider.GetRequiredService<IMapper<TEntity, TDto>>();
        return mapper.Map(ent);
    }

    protected virtual async Task<IQueryResultDto<TDto>> MapResult<TDto>(
        IMapper<TEntity, TDto> mapper,
        QueryResult<TEntity> queryResult,
        CancellationToken ct)
        where TDto : class
    {
        var dtos = queryResult.Count == 0
            ? new List<TDto>()
            : await mapper.ProjectAsync(queryResult.Items, true, ct).ToListAsync(ct);

        var res = new QueryResultDto<TDto> { Count = queryResult.Count, Items = dtos };
        return res;
    }
}