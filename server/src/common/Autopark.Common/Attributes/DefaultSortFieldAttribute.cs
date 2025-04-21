namespace Autopark.Common.Attributes;

/// <summary>
///     Атрибут для выбора поля для сортировки по умолчанию. Если не использовать этот атрибут, то будет использоваться Id
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DefaultSortFieldAttribute : Attribute
{
    public DefaultSortFieldAttribute(string field)
    {
        Field = field;
    }

    /// <summary>
    ///     Поле для сортировки по умолчанию.
    /// </summary>
    public string Field { get; }
}
