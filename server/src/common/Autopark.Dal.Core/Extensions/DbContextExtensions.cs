using Autopark.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace Autopark.Dal.Core.Extensions;

public static class DbContextExtensions
{
    /// <summary>
    ///     Присоеденить к контексту множество сущностей
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TId">Тип идентификатора ключа сущности</typeparam>
    /// <param name="dbContext">Контекст БД</param>
    /// <param name="ids">Id сущностей</param>
    /// <param name="groupId">Id группы</param>
    /// <returns>Присоединенные сущности</returns>
    public static IReadOnlyCollection<TEntity> AttachRange<TEntity>(
        this DbContext dbContext,
        IEnumerable<Guid> ids,
        Guid groupId)
        where TEntity : class, IEntityBase, new()
    {
        var attachedEntitites = dbContext.Set<TEntity>()
            .Local
            .Where(t => ids.Any(x => x.Equals(t.Id)))
            .ToList();

        var notAttachedIds = ids.Where(x => !attachedEntitites.Any(a => a.Id.Equals(x))).ToArray();

        var unutachedList = new List<TEntity>();

        foreach (var notAttachedId in notAttachedIds)
        {
            var entity = new TEntity();

            entity.Id = notAttachedId;
            entity.GroupId = groupId;

            unutachedList.Add(entity);
        }

        if (unutachedList.Any())
        {
            dbContext.AttachRange(unutachedList);
            attachedEntitites.AddRange(unutachedList);
        }

        return attachedEntitites;
    }

    public static string[] GetKeyNames<T>(this DbContext context)
        where T : class
    {
        return context.Model.FindEntityType(typeof(T)).GetKeys()
            .SelectMany(x =>
                x.Properties.Select(y => y.Name)).ToArray();
    }
}