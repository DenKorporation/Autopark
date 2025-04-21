using Autopark.Dal.Core.Filters;

namespace Autopark.Dal.Core.Extensions;

public static class FilterExtensions
{
    public static IRangeFilterEx ToRangeFilterEx(this IRangeFilter rangeFilter)
    {
        var rangFilterEx = (IRangeFilterEx)Activator.CreateInstance(typeof(RangeFilterEx<>)
            .MakeGenericType(rangeFilter.ValueType));
        rangFilterEx.UseAsSingleValue = rangeFilter.UseAsSingleValue;
        rangFilterEx.IsNotNull = rangeFilter.IsNotNull;
        rangFilterEx.End = rangeFilter.End;
        rangFilterEx.Start = rangeFilter.Start;
        rangFilterEx.EndIsExclusive = rangeFilter.IsExclusive;
        rangFilterEx.StartIsExclusive = rangeFilter.IsExclusive;
        return rangFilterEx;
    }
}
