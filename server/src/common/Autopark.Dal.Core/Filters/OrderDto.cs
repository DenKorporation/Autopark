namespace Autopark.Dal.Core.Filters;

/// <summary>
///     Сортировка
/// </summary>
public class OrderDto
{
    public OrderDto()
    {
    }

    public OrderDto(bool? desc, string field)
    {
        Desc = desc;
        Field = field;
    }

    /// <summary>
    ///     По убыванию
    /// </summary>
    public bool? Desc { get; set; }

    /// <summary>
    ///     Поле сортировки
    /// </summary>
    public string Field { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{nameof(Desc)}: {Desc}, {nameof(Field)}: {Field}";
    }
}