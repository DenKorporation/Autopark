using Autopark.Common.Domain;
using Autopark.Common.Mapping;
using Autopark.Common.Security;
using Autopark.Dal.Core.Filters;
using Autopark.Dal.Core.QueryTransformers;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Autopark.Dal.Core.Repositories;


/// <inheritdoc cref="IEditDtoRepository{TEntity, TDto}" />
public class DefaultEditDtoRepository<TEntity, TDto> : DefaultDtoRepository<TEntity, TDto>,
    IEditDtoRepository<TEntity, TDto>
    where TEntity : class, IEntityBase, new()
    where TDto : class, IEntityDto, new()
{
    private readonly IMapper<TDto, TEntity> entityMapper;

    /// <inheritdoc />
    public DefaultEditDtoRepository(
        DbContext dbContext,
        IMapper<TEntity, TDto> mapper,
        ILogger<DefaultEditDtoRepository<TEntity, TDto>> logger,
        IUserInfoProvider userInfoProvider,
        IMapper<TDto, TEntity> entityMapper,
        IQueryTransformer<TEntity> queryTransformer)
        : base(dbContext, mapper, logger, userInfoProvider, queryTransformer)
    {
        this.entityMapper = entityMapper;
    }

    /// <inheritdoc />
    public Task DeleteByFilterAsync<TFilter>(TFilter filter, CancellationToken ct = default)
        where TFilter : class, new()
    {
        return CreateQueryable(new QueryFilter<TFilter>
            {
                Filter = filter
            })
            .ExecuteDeleteAsync(ct);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Guid>> SaveDtoRangeAsync(
        IReadOnlyList<TDto> dtos,
        bool flush = true,
        bool forceInsert = false,
        CancellationToken cancellationToken = default)
    {
        return await SaveDtoRangeAsync(dtos, flush, forceInsert, null, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Guid>> SaveDtoRangeImmediatelyAsync(
        IReadOnlyList<TDto> dtos,
        bool forceInsert = false,
        bool assignIdsToDtos = false,
        CancellationToken cancellationToken = default)
    {
        Action<TEntity, TDto> callback = assignIdsToDtos
            ? (entity, dto) => dto.Id = entity.Id
            : null;

        return await SaveDtoRangeAsync(
            dtos,
            true,
            forceInsert,
            callback,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Guid> FindByUniqueFilterOrSaveDtoAsync<TFilter>(
        TFilter uniqueFilter,
        TDto dto,
        bool flush = true,
        bool forceInsert = false,
        CancellationToken cancellationToken = default)
        where TFilter : class, new()
    {
        async Task<Guid?> FindByUniqueFilterAsync()
        {
            var uniqueFilterQuery = CreateQueryable(new QueryFilter<TFilter>
            {
                WithCount = false,
                DoNotOrder = true,
                Take = 1,
                Filter = uniqueFilter
            });

            return await uniqueFilterQuery.Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
        }

        var existingId = await FindByUniqueFilterAsync();
        if (existingId != null)
        {
            return existingId.Value;
        }

        try
        {
            var entityId = await SaveDtoAsync(dto, flush, forceInsert, cancellationToken);
            return entityId;
        }
        catch (UniqueConstraintException e)
        {
            var entityId = await FindByUniqueFilterAsync()
                           ?? throw new InvalidOperationException(
                               "Не удалось получить Id сущности после UniqueConstraintException при попытке его создания",
                               e);

            return entityId;
        }
    }

    /// <inheritdoc />
    public Task<Guid> SaveDtoAsync(
        TDto dto,
        bool flush = true,
        bool forceInsert = false,
        CancellationToken cancellationToken = default)
    {
        return SaveDtoAsync(dto, MapToEntity, flush, forceInsert, cancellationToken);
    }

    private async Task<Guid> SaveDtoAsync<TTDTo>(
        TTDTo dto,
        Action<TTDTo, TEntity> mapFunc,
        bool flush = true,
        bool forceInsert = false,
        CancellationToken cancellationToken = default)
        where TTDTo : TDto
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var mustInsert = dto.Id == default || forceInsert;

        TEntity entity = null;
        if (!mustInsert)
        {
            entity = await GetEntityAsync<TEntity>(dto.Id, cancellationToken);
        }

        if (entity == null)
        {
            entity = new TEntity();
            mustInsert = true;
        }

        mapFunc(dto, entity);

        if (mustInsert)
        {
            await MarkAddEntityAsync(entity, cancellationToken);
        }

        if (entityMapper is IAfterMapper<TDto, TEntity> afterMapper)
        {
            afterMapper.AfterMap(dto, entity);
        }

        if (flush)
        {
            await FlushAsync(cancellationToken);
        }

        return entity.Id;
    }

    private void MapToEntity(TDto dto, TEntity entity)
    {
        entityMapper.Map(dto, entity);
    }

    private async Task<IReadOnlyList<Guid>> SaveDtoRangeAsync(
        IReadOnlyList<TDto> dtos,
        bool flush,
        bool forceInsert,
        Action<TEntity, TDto> afterFlushCallBack,
        CancellationToken cancellationToken)
    {
        var maxBatchSize = 2000;

        var dtosForInsertBatches = forceInsert ? dtos.Chunk(maxBatchSize).ToList() : dtos.Where(x => x.Id == null).Chunk(maxBatchSize).ToList();
        var dtoForUpdateBatches = forceInsert ? new TDto[] { }.Chunk(maxBatchSize) : dtos.Where(x => x.Id != null).Chunk(maxBatchSize);

        async IAsyncEnumerable<(TDto, TEntity)> UpdateEntities()
        {
            foreach (var updateDtoBacth in dtoForUpdateBatches)
            {
                if (!updateDtoBacth.Any())
                {
                    continue;
                }

                var updateDic = updateDtoBacth.ToDictionary(x => x.Id);

                var entities = await Query<TEntity>()
                    .Where(x => updateDic.Keys.Contains(x.Id))
                    .ToArrayAsync(cancellationToken);
                var entitiesIds = entities.Select(x => x.Id);

                var entititesForAdd = updateDic.Values
                    .Where(x => !entitiesIds.Contains(x.Id))
                    .ToArray();

                if (entititesForAdd.Any())
                {
                    dtosForInsertBatches.Add(entititesForAdd);
                }

                foreach (var entity in entities)
                {
                    var dto = updateDic[entity.Id];
                    MapToEntity(dto, entity);
                    yield return (dto, entity);
                }
            }
        }

        IEnumerable<(TDto, TEntity)> AddEntities()
        {
            var entities = new List<TEntity>();

            foreach (var insertDtoBatch in dtosForInsertBatches)
            {
                if (!insertDtoBatch.Any())
                {
                    continue;
                }

                foreach (var x in insertDtoBatch)
                {
                    var entity = new TEntity();
                    MapToEntity(x, entity);
                    entities.Add(entity);
                    yield return (x, entity);
                }
            }

            MarkAddRange(entities);
        }

        var updatedEntitites = await UpdateEntities()
            .ToListAsync(cancellationToken);

        var addedEntities = AddEntities()
            .ToArray();

        if (flush)
        {
            await FlushAsync(cancellationToken);

            if (afterFlushCallBack != null)
            {
                foreach (var entity in updatedEntitites.Union(addedEntities))
                {
                    afterFlushCallBack(entity.Item2, entity.Item1);
                }
            }
        }

        return updatedEntitites
            .Select(x => x.Item2.Id)
            .Union(addedEntities
                .Select(x => x.Item2.Id))
            .ToArray();
    }
}