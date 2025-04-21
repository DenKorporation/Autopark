using Autopark.Common.Helpers;
using Newtonsoft.Json;

namespace Autopark.Dal.Core.Filters;

public class RangeFilterEx<T> : ICloneable, IRangeFilterEx
    where T : struct
{
        /// <summary>
        ///     Пустой фильтр
        /// </summary>
        public static readonly RangeFilterEx<T> Empty = new();

        private bool isNotNull;

        private bool isNull;

        /// <inheritdoc />
        [JsonIgnore]
        public Type ValueType => typeof(T);

        public virtual T? End { get; set; }

        /// <inheritdoc />
        public bool StartIsExclusive { get; set; }

        /// <inheritdoc />
        public bool EndIsExclusive { get; set; }

        /// <inheritdoc />
        public bool IsNotNull
        {
            get => isNotNull;

            set
            {
                isNotNull = value;
                if (isNotNull)
                {
                    isNull = false;
                }
            }
        }

        /// <inheritdoc />
        public bool IsNull
        {
            get => isNull;

            set
            {
                isNull = value;
                if (isNull)
                {
                    isNotNull = false;
                }
            }
        }

        /// <summary>
        ///     Начало диапазона, включительно
        /// </summary>
        public T? Start { get; set; }

        /// <inheritdoc />
        public bool UseAsSingleValue { get; set; }

        /// <inheritdoc />
        object IRangeFilterEx.End
        {
            get => End;
            set => End = (T?)value;
        }

        /// <inheritdoc />
        object IRangeFilterEx.Start
        {
            get => Start;
            set => Start = (T?)value;
        }

        /// <summary>
        ///     Приводит одиночное значение к типу этого фильтра
        /// </summary>
        public static implicit operator RangeFilterEx<T>(T value)
        {
            return new RangeFilterEx<T> { Start = value, UseAsSingleValue = true };
        }

        /// <summary>
        ///     Приводит одиночное значение к типу этого фильтра
        /// </summary>
        public static explicit operator RangeFilterEx<T>(RangeFilter<T> value)
        {
            return new RangeFilterEx<T>
            {
                EndIsExclusive = value.IsExclusive,
                StartIsExclusive = value.IsExclusive,
                End = value.End,
                Start = value.Start,
                UseAsSingleValue = value.UseAsSingleValue,
                IsNotNull = value.IsNotNull,
                IsNull = value.IsNull
            };
        }

        /// <inheritdoc />
        public object Clone()
        {
            return ReflectionHelper.Clone(this);
        }

        /// <inheritdoc />
        public bool HasValue()
        {
            return Start.HasValue || End.HasValue || IsNull || IsNotNull;
        }

        /// <inheritdoc />
        public void Reset()
        {
            Start = null;
            End = null;
            IsNotNull = false;
            IsNull = false;
            UseAsSingleValue = false;
            EndIsExclusive = false;
            StartIsExclusive = false;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return
                !HasValue()
                    ? "Empty"
                    : $"Start: {Start}, End: {End}, UseAsSingleValue: {UseAsSingleValue}, IsNull: {IsNull}, IsNotNull: {IsNotNull}, HasValue: {HasValue()}";
        }
}
