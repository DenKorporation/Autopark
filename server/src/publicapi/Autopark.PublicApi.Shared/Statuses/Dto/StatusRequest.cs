using Autopark.Common.Domain;

namespace Autopark.PublicApi.Shared.Statuses.Dto;

public class StatusRequest : EntityDto
{
    public string Name { get; set; }
}