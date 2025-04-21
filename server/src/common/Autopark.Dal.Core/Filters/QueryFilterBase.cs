using System.Text.Json.Serialization;
using Autopark.Common.Extensions;

namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Настройки запроса
/// </summary>
public abstract class QueryFilterBase
{
    /// <summary>
    /// Настройки запроса
    /// </summary>
    protected QueryFilterBase()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Order = new List<OrderDto>();
    }

    /// <summary>
    ///     Настройка сортировки
    /// </summary>
    public virtual List<OrderDto> Order { get; set; }

    /// <summary>
    /// Не сортировать результаты
    /// </summary>
    public virtual bool DoNotOrder { get; set; }

    /// <summary>
    ///     Отступ выборки
    /// </summary>
    public int? Skip { get; set; }

    /// <summary>
    ///     Кол-во записей для выборки
    /// </summary>
    public int? Take { get; set; }

    /// <summary>
    /// Включает режим расширенных ограничений
    /// </summary>
    [JsonIgnore]
    public virtual bool EnablePrivateMode { get; set; }

    /// <summary>
    ///     Получение поля с общим кол-вом записей по фильтру
    /// </summary>
    public bool? WithCount { get; set; }

    /// <summary>
    ///     Получить в результате только количество элементов
    /// </summary>
    public bool OnlyCount { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return
            $"{nameof(Order)}: {Order.ToString(",")}, {nameof(Skip)}: {Skip}, {nameof(Take)}: {Take}," +
            $" {nameof(WithCount)}: {WithCount}, {nameof(OnlyCount)}: {OnlyCount}";
    }
}