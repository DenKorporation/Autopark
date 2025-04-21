using Autopark.Dal.Core.Filters;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Autopark.Common.Bl.Validators;

public class QueryFilterValidator<TFilter>
    : AbstractValidator<QueryFilter<TFilter>>
    where TFilter : class, new()
{
    public QueryFilterValidator(IServiceProvider serviceProvider)
    {
        RuleFor(x => x.Skip)
            .NotNull()
            .WithMessage("The page number was expected")
            .GreaterThanOrEqualTo(0)
            .WithMessage("The page number must be more than or equal to 0");

        RuleFor(x => x.Take)
            .NotNull()
            .WithMessage("The page size was expected")
            .GreaterThanOrEqualTo(0)
            .WithMessage("The page size must be more than or equal to 0");

        var filterValidator = serviceProvider.GetService<IValidator<TFilter>>();

        if (filterValidator is not null)
        {
            RuleFor(x => x.Filter)
                .SetValidator(filterValidator);  
        } 
    }
}