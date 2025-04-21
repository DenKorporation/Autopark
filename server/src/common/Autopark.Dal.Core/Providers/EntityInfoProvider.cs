using System.Reflection;
using Autopark.Common.Attributes;
using Autopark.Common.Domain;
using Autopark.Dal.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Autopark.Dal.Core.Providers;

/// <inheritdoc />
[ServiceAsInterfaces]
internal sealed class EntityInfoProvider : IEntityInfoProvider
{
    private readonly DbContext dbContext;

    public EntityInfoProvider(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc />
    public IReadOnlyList<string> GetDefaultSortFields<TEntity>()
        where TEntity : class, IEntityBase
    {
        var defaultSortField = GetDefaultSortAttribute<TEntity>();
        return defaultSortField == null
            ? dbContext.GetKeyNames<TEntity>()
            : new[] { defaultSortField.Field };
    }

    /// <inheritdoc />
    public bool HasCustomSortField<TEntity>()
        where TEntity : class, IEntityBase
    {
        return GetDefaultSortAttribute<TEntity>() != null;
    }

    private DefaultSortFieldAttribute GetDefaultSortAttribute<TEntity>()
        where TEntity : class, IEntityBase
    {
        return typeof(TEntity).GetCustomAttribute<DefaultSortFieldAttribute>();
    }
}
