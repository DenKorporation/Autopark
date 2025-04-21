using System.Diagnostics;

namespace Autopark.Common.Domain;

[DebuggerDisplay("Id: {Id}, Type: {GetType()}")]
public class EntityDto : IEntityDto
{
    public Guid Id { get; set; }
}
