using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.EventTypes.Dto;

public class EventTypeFilterDto
{
    public StringFilter Name { get; set; } = new();
}