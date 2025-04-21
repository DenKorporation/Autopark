namespace Autopark.Common.Domain;

public interface IEntityBase
{
    Guid Id { get; set; }
    Guid GroupId { get; set; }
}
