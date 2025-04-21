using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Autopark.Common.Domain;

[DebuggerDisplay("Id: {Id}, Type: {GetType()}")]
public abstract class EntityBase : IEntityBase
{
    [Key]
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
}
