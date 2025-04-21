namespace Autopark.Common.Mapping;

/// <summary>The Mapper interface.</summary>
public interface IMapper
{
    /// <summary>The map.</summary>
    /// <param name="input">The input.</param>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    TOutput Map<TInput, TOutput>(TInput input);

    /// <summary>The map.</summary>
    /// <param name="input">The input.</param>
    /// <param name="targetType">Target type.</param>
    /// <returns></returns>
    object Map(object input, Type targetType);

    /// <summary>The map.</summary>
    /// <param name="input">The input.</param>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    TOutput Map<TOutput>(object input);

    /// <summary>The assert configuration is valid.</summary>
    void AssertConfigurationIsValid();

    /// <summary>The map.</summary>
    /// <param name="input">The input.</param>
    /// <param name="output">The output.</param>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    TOutput Map<TInput, TOutput>(TInput input, TOutput output);

    /// <summary>The project to.</summary>
    /// <param name="input">The input.</param>
    /// <typeparam name="TOutput"></typeparam>
    /// <returns></returns>
    IQueryable<TOutput> ProjectTo<TOutput>(IQueryable input);
}

/// <summary>The Mapper interface.</summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TOutput"></typeparam>
public interface IMapper<in TInput, out TOutput> : IMapper
{
    /// <summary>
    ///     Mapping dto from entity and automatically calls AfterMap
    /// </summary>
    TOutput Map(TInput input);

    /// <summary>
    /// Маппит коллекции сущностей
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    TOutput[] Map(IEnumerable<TInput> input);

    /// <summary>The project.</summary>
    /// <param name="input">The input.</param>
    /// <returns></returns>
    IQueryable<TOutput> Project(IQueryable<TInput> input);

    /// <summary>
    ///     Mapping dto's from IQueryable
    /// </summary>
    /// <param name="inputQuery">IQueryable</param>
    /// <param name="enableDeepMapping">Setup automatically calls AfterMap</param>
    /// <returns></returns>
    TOutput[] Project(IQueryable<TInput> inputQuery, bool enableDeepMapping);

    /// <summary>
    ///     Получает проекции из IQueryable
    /// </summary>
    /// <param name="inputQuery">IQueryable</param>
    /// <param name="enableDeepMapping">Вызывать AfterMap для полученных проекций</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    IAsyncEnumerable<TOutput> ProjectAsync(
        IQueryable<TInput> inputQuery,
        bool enableDeepMapping,
        CancellationToken cancellationToken = default);
}