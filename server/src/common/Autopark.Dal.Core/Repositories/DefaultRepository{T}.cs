using Autopark.Common.Domain;
using Autopark.Common.Security;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.Queries;
using Autopark.Dal.Core.QueryTransformers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Autopark.Dal.Core.Repositories;

public class DefaultRepository<TEntity> : DefaultRepository, IRepository<TEntity>
    where TEntity : class, IEntityBase, new()
{
    private readonly IQueryTransformer<TEntity> queryTransformer;
    private readonly DbContext dbContext;

    public DefaultRepository(
        DbContext dbContext,
        ILogger<DefaultRepository<TEntity>> logger,
        IUserInfoProvider userInfoProvider,
        IQueryTransformer<TEntity> queryTransformer)
        : base(dbContext, logger, userInfoProvider)
    {
        this.dbContext = dbContext;
        this.queryTransformer = queryTransformer;
    }

    /// <inheritdoc />
    public Task<bool> AnyByFilterAsync<TFilter>(TFilter filter, CancellationToken ct = default)
        where TFilter : class, new()
    {
        return CreateQueryable(new QueryFilter<TFilter>
        {
            Filter = filter
        }).AnyAsync(ct);
    }

    /// <inheritdoc />
    public bool AnyByFilter<TFilter>(TFilter filter)
        where TFilter : class, new()
    {
        return CreateQueryable(new QueryFilter<TFilter>
        {
            Filter = filter
        }).Any();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var entity = Attach<TEntity>(id);
        MarkDeleteEntity(entity);

        await FlushAsync();
    }

    public Task<QueryResult<TEntity>> QueryAsync<TFilter>(
        QueryFilter<TFilter> filter,
        CancellationToken ct = default)
        where TFilter : class, new()
    {
        return queryTransformer.GetQueryableResultAsync(filter, Query<TEntity>, dbContext, ct);
    }

    public QueryResult<TEntity> Query<TFilter>(QueryFilter<TFilter> filter)
        where TFilter : class, new()
    {
        return queryTransformer.GetQueryableResult(filter, Query<TEntity>, dbContext);
    }

    protected internal virtual IQueryable<TEntity> CreateQueryable<TFilter>(QueryFilter<TFilter> filter)
        where TFilter : class, new()
    {
        return queryTransformer.CreateQueryable(filter, Query<TEntity>);
    }
}