using System.Linq.Expressions;
using System.Reflection;
using Autopark.Common.Domain;
using Autopark.Common.Security;
using Autopark.Dal.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Autopark.Dal.Core.Repositories;

public class DefaultRepository : IRepository
{
    public DefaultRepository(
        DbContext dbContext,
        ILogger<DefaultRepository> logger,
        IUserInfoProvider userInfoProvider)
    {
        DbContext = dbContext;
        Logger = logger;
        UserInfoProvider = userInfoProvider;
    }

    protected DbContext DbContext { get; }
    
    protected ILogger Logger { get; }

    protected IUserInfoProvider UserInfoProvider { get; }
    
    protected Guid GroupId => UserInfoProvider.GetGroupId();

    public IQueryable<TEntity> Query<TEntity>()
        where TEntity : class, IEntityBase, new()
    {
        return DbContext.Set<TEntity>().FilterByGroup(UserInfoProvider);
    }

    /// <inheritdoc />
    public Task<TEntity?> GetEntityAsync<TEntity>(Guid keyValues, CancellationToken cancellationToken = default)
        where TEntity : class, IEntityBase, new()
    {
        return DbContext.Set<TEntity>().FilterByGroup(UserInfoProvider).FirstOrDefaultAsync(x => x.Id.Equals(keyValues), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TEntity> AddEntityAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
    {
        entity.GroupId = GroupId;
        var ent = await DbContext.AddAsync(entity, ct);
        await DbContext.SaveChangesAsync(ct);
        return ent.Entity;
    }

    /// <inheritdoc />
    public Task UpdateEntityAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
    {
        entity.GroupId = GroupId;
        DbContext.Update(entity);
        return DbContext.SaveChangesAsync(ct);
    }

    /// <inheritdoc />
    public Task UpdateRangeAsync<TEntity>(IEnumerable<TEntity> list, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
    {
        var entityBases = list.ToList();
        entityBases.ForEach(x => x.GroupId = GroupId);
        DbContext.UpdateRange(entityBases);
        return DbContext.SaveChangesAsync(ct);
    }

    /// <inheritdoc />
    public async Task MarkAddEntityAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
    {
        entity.GroupId = GroupId;
        await DbContext.AddAsync(entity, ct);
    }

    /// <inheritdoc />
    public virtual void MarkAddRange<TEntity>(IEnumerable<TEntity> list)
        where TEntity : class, IEntityBase, new()
    {
        var entityBases = list.ToList();
        entityBases.ForEach(x => x.GroupId = GroupId);
        DbContext.AddRange(entityBases);
    }

    /// <inheritdoc />
    public void MarkDeleteEntity<TEntity>(TEntity entity)
        where TEntity : class, IEntityBase, new()
    {
        if (entity.GroupId != GroupId)
        {
            throw new InvalidOperationException("Cannot delete an entity out of a current group.");
        }

        DbContext.Remove(entity);
    }

    /// <inheritdoc />
    public virtual void MarkDeleteRange<TEntity>(IEnumerable<TEntity> list)
        where TEntity : class, IEntityBase, new()
    {
        var entityBases = list.ToList();

        if (entityBases.Any(x => x.GroupId != GroupId))
        {
            throw new InvalidOperationException("Cannot delete an entity out of a current group.");
        }

        DbContext.RemoveRange(entityBases);
    }

    /// <inheritdoc />
    public virtual void MarkUpdateEntity<TEntity>(TEntity entity)
        where TEntity : class, IEntityBase, new()
    {
        entity.GroupId = GroupId;
        DbContext.Update(entity);
    }

    /// <inheritdoc />
    public virtual void MarkUpdateRange<TEntity>(IEnumerable<TEntity> list)
        where TEntity : class, IEntityBase, new()
    {
        var entityBases = list.ToList();
        entityBases.ForEach(x => x.GroupId = GroupId);
        DbContext.UpdateRange(entityBases);
    }

    /// <inheritdoc />
    public async Task LoadPropertyAsync<TEntity, TProp>(
        TEntity entity,
        Expression<Func<TEntity, TProp>> navExpression,
        CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
        where TProp : class
    {
        await DbContext.Entry(entity).Reference(navExpression).LoadAsync(ct);
    }

    /// <inheritdoc />
    public async Task LoadPropertyAsync(object entity, PropertyInfo property, CancellationToken ct = default)
    {
        await DbContext.Entry(entity).Reference(property.Name).LoadAsync(ct);
    }

    /// <inheritdoc />
    public async Task ReloadEntityAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
    {
        await DbContext.Entry(entity).ReloadAsync(ct);
    }

    /// <inheritdoc />
    public async Task LoadCollectionAsync<TEntity, TProp>(
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProp>>> navExpression,
        CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
        where TProp : class
    {
        await DbContext.Entry(entity).Collection(navExpression).LoadAsync(ct);
    }

    /// <inheritdoc />
    public async Task LoadCollectionAsync(object entity, PropertyInfo propCollection, CancellationToken ct = default)
    {
        await DbContext
            .Entry(entity)
            .Collection(propCollection.Name)
            .LoadAsync(ct);
    }

    /// <inheritdoc />
    public virtual async Task FlushAsync(CancellationToken ct = default, bool withClear = false)
    {
        await DbContext.SaveChangesAsync(ct);

        if (withClear)
        {
            DbContext.ChangeTracker.Clear();
        }
    }

    /// <inheritdoc />
    public Task DeleteAsync<TEntity>(TEntity entity, CancellationToken ct = default)
        where TEntity : class, IEntityBase, new()
    {
        if (entity.GroupId != GroupId)
        {
            throw new InvalidOperationException("Cannot delete an entity out of a current group.");
        }

        DbContext.Remove(entity);
        return DbContext.SaveChangesAsync(ct);
    }

    /// <inheritdoc />
    public void Detach<TEntity>(TEntity entity)
        where TEntity : class, IEntityBase, new()
    {
        DbContext.Entry(entity).State = EntityState.Detached;
    }

    /// <inheritdoc />
    public void DetachById<TEntity>(Guid id)
        where TEntity : class, IEntityBase, new()
    {
        var entity = DbContext.Set<TEntity>().Local.FirstOrDefault(t => t.Id.Equals(id));

        if (entity != null)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }
    }

    /// <inheritdoc />
    public TEntity Attach<TEntity>(Guid id)
        where TEntity : class, IEntityBase, new()
    {
        var entity = DbContext.Set<TEntity>().Local.FirstOrDefault(t => t.Id.Equals(id));

        if (entity == null)
        {
            entity = new TEntity();

            entity.Id = id;
            entity.GroupId = GroupId;

            DbContext.Attach(entity);
        }

        return entity;
    }

    /// <inheritdoc />
    public IReadOnlyCollection<TEntity> AttachRange<TEntity>(IEnumerable<Guid> ids)
        where TEntity : class, IEntityBase, new()
    {
        return DbContext.AttachRange<TEntity>(ids, GroupId);
    }
}