using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Autopark.Common.Domain;

/// <summary>
///     Базовый интерфейс dto для сущности
/// </summary>
public interface IEntityDto
{
    /// <summary>
    ///     Id сущности
    /// </summary>
    [Display(Name = "Идентификатор")]
    [DisplayName("Идентификатор")]
    Guid Id { get; set; }
}
