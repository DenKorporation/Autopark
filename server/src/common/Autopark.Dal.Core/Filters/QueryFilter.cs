using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Autopark.Dal.Core.Filters;


/// <summary>
///     Базовый класс для фильтров запросов
/// </summary>
/// <typeparam name="TFilter">Тип фильтра</typeparam>
public class QueryFilter<TFilter> : QueryFilterBase
    where TFilter : class, new()
{
    /// <inheritdoc />
    public QueryFilter()
    {
        Filter = new TFilter();
        FilterMode = QueryFilterMode.And;
    }

    /// <inheritdoc />
    public QueryFilter(TFilter filter)
    {
        Filter = filter;
    }

    /// <summary>
    ///     Фильтр запроса, детальное описание полей указано в модели
    /// </summary>
    public virtual TFilter Filter { get; set; }

    /// <summary>
    ///     Способ объединения выражений полученных из фильтра запроса. 'И' по умолчанию
    /// </summary>
    [JsonIgnore]
    public QueryFilterMode FilterMode { get; set; }

    /// <summary>
    ///     Поля, которые необходимо наджойнить
    /// </summary>
    [JsonIgnore]
    public string[] Includes { get; set; }

    /// <summary>
    ///     Поля, которые необходимо исключить
    /// </summary>
    [JsonIgnore]
    public string[] Excludes { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(Filter)}: {Filter}, {nameof(Includes)}: {Includes}, {nameof(Excludes)}: {Excludes}";
    }
}