using Autopark.Common.Domain;
using Autopark.Common.Mapping;
using Autopark.Common.Security;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.QueryTransformers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Autopark.Dal.Core.Repositories;


public class DefaultDtoRepository<TEntity, TDto> : DefaultRepository<TEntity>, IDtoRepository<TEntity, TDto>
    where TEntity : class, IEntityBase, new()
    where TDto : class, new()
{
    public DefaultDtoRepository(
        DbContext dbContext,
        IMapper<TEntity, TDto> mapper,
        ILogger<DefaultDtoRepository<TEntity, TDto>> logger,
        IUserInfoProvider userInfoProvider,
        IQueryTransformer<TEntity> queryTransformer)
        : base(dbContext, logger, userInfoProvider, queryTransformer)
    {
        Mapper = mapper;
    }

    public IMapper<TEntity, TDto> Mapper { get; }

    public virtual async Task<TDto?> GetDtoAsync(Guid id, CancellationToken ct = default)
    {
        // var query = Query<TEntity>().Where(x => x.Id == id);
        // var entities = await Mapper.ProjectAsync(query, true, ct)
        //                            .FirstOrDefaultAsync(ct);

        // return entities;

        var ent = await GetEntityAsync<TEntity>(id, ct);
        return Mapper.Map(ent);
    }

    public virtual TDto MapEntity(TEntity entity)
    {
        return Mapper.Map<TDto>(entity);
    }

    public virtual async Task<QueryResultDto<TDto>> QueryDtoAsync<TFilter>(QueryFilter<TFilter> queryDto, CancellationToken ct = default)
        where TFilter : class, new()
    {
        var query = await QueryAsync(queryDto, ct);
        var dtos = query.Count == 0
            ? new List<TDto>()
            : await Mapper
                .ProjectAsync(query.Items, true, ct)
                .ToListAsync(ct);

        var res = new QueryResultDto<TDto> { Count = query.Count, Items = dtos };
        return res;
    }

    public virtual QueryResultDto<TDto> QueryDto<TFilter>(QueryFilter<TFilter> queryDto)
        where TFilter : class, new()
    {
        var query = Query(queryDto);
        var dtos = Mapper.Project(query.Items, true);

        var res = new QueryResultDto<TDto> { Count = query.Count, Items = dtos };
        return res;
    }
}
