using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.EventTypes.Dto;

public class EventTypeRequest : EntityDto
{
    public string Name { get; set; }
}