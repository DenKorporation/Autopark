using Autopark.Common.Mapping;
using Microsoft.EntityFrameworkCore;

using IMapper = AutoMapper.IMapper;

namespace Autopark.Common.Bl.Mappers;

public class DefaultMapper<TInput, TOutput> : DefaultMapper, IExtendedMapper<TInput, TOutput>, IAfterMapper<TInput, TOutput>
//where TInput : class, IEntityBase, new()
{
    public DefaultMapper(IMapper mapper)
        : base(mapper)
    {
    }

    public virtual TOutput Map(TInput input)
    {
        if (input == null)
        {
            return default(TOutput);
        }

        var result = Mapper.Map<TOutput>(input);
        AfterMap(input, result);
        return result;
    }

    public TOutput[] Map(IEnumerable<TInput> input)
    {
        var result = Mapper.Map<TOutput[]>(input);
        AfterMap(input, result);
        return result;
    }

    public virtual IQueryable<TOutput> Project(IQueryable<TInput> inputQuery)
    {
        if (inputQuery == null)
        {
            return null;
        }

        return Mapper.ProjectTo<TOutput>(inputQuery);
    }

    public virtual TOutput[] Project(
        IQueryable<TInput> inputQuery,
        bool enableDeepMapping)
    {
        if (inputQuery == null)
        {
            return null;
        }

        var result = Project(inputQuery).ToArray();

        if (enableDeepMapping)
        {
            AfterMap(inputQuery, result);
        }

        return result;
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<TOutput> ProjectAsync(
        IQueryable<TInput> inputQuery,
        bool enableDeepMapping,
        CancellationToken cancellationToken = default)
    {
        if (inputQuery == null)
        {
            yield break;
        }

        var result = await Project(inputQuery)
            .ToArrayAsync(cancellationToken);

        if (enableDeepMapping)
        {
            AfterMap(inputQuery, result);
        }

        foreach (var item in result)
        {
            yield return item;
        }
    }

    /// <inheritdoc />
    public void Map(TInput input, TOutput output, bool useAfterMap)
    {
        Map(input, output);

        if (useAfterMap)
        {
            AfterMap(input, output);
        }
    }

    public virtual void AfterMap(IQueryable<TInput> inputQuery, IEnumerable<TOutput> existingModels)
    {
    }

    public virtual void AfterMap(IEnumerable<TInput> inputQuery, IEnumerable<TOutput> existingModels)
    {
    }

    public virtual void AfterMap(TInput input, TOutput existingModel)
    {
    }
}
