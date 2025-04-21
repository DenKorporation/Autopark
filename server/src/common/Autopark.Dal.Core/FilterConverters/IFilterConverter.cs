using System.Linq.Expressions;
using Autopark.Dal.Core.Filters;

namespace Autopark.Dal.Core.FilterConverters;

public interface IFilterConverter
{
    public delegate IFilterConverter Factory(Type entity, Type filter);

    Expression<Func<TModel, bool>> CreateFilterExpression<TModel, TFilter>(QueryFilter<TFilter> filter)
        where TFilter : class, new()
        where TModel : class, new();
}