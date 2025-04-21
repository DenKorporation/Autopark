using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Groups.Dto;

public class GroupResponse : EntityDto
{
    public string Name { get; set; }
    public string Path { get; set; }
    public Guid? ParentId { get; set; }
}
