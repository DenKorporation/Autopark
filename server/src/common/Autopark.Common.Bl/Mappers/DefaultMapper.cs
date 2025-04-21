using Autopark.Common.Mapping;

namespace Autopark.Common.Bl.Mappers;

public class DefaultMapper : IMapper
{
    public DefaultMapper(AutoMapper.IMapper mapper)
    {
        Mapper = mapper;
    }

    protected AutoMapper.IMapper Mapper { get; }

    public TOutput Map<TInput, TOutput>(TInput input)
    {
        return Mapper.Map<TOutput>(input);
    }

    public object Map(object input, Type targetType)
    {
        return Mapper.Map(input, input.GetType(), targetType);
    }

    public TOutput Map<TOutput>(object input)
    {
        return Mapper.Map<TOutput>(input);
    }

    public TOutput Map<TInput, TOutput>(TInput input, TOutput output)
    {
        return Mapper.Map(input, output);
    }

    public IQueryable<TOutput> ProjectTo<TOutput>(IQueryable input)
    {
        return Mapper.ProjectTo<TOutput>(input);
    }

    public void AssertConfigurationIsValid()
    {
        Mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}