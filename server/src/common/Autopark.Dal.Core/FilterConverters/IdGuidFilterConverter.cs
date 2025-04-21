using Autopark.Common.Domain;
using Autopark.Dal.Core.Filters;
using Microsoft.Extensions.Logging;

namespace Autopark.Dal.Core.FilterConverters;

public class IdGuidFilterConverter<TModel> : FilterConverterBase<TModel, IdFilterDto<Guid>>
    where TModel : class, IEntityBase, new()
{
    /// <inheritdoc />
    public IdGuidFilterConverter(ILogger<IdGuidFilterConverter<TModel>> logger)
        : base(logger)
    {
        HandleField(x => x.Ids, x => x.Id);
    }
}