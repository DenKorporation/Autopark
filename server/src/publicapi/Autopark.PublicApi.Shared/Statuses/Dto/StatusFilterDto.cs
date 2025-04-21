using Autopark.Dal.Core.Filters;

namespace Autopark.PublicApi.Shared.Statuses.Dto;

public class StatusFilterDto
{
    public StringFilter Name { get; set; } = new();
}