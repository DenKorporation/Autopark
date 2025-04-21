using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ValidationException = Autopark.Common.Exceptions.ValidationException;

namespace Autopark.Common.Validations;

public class DefaultModelValidator<TModel>(IServiceProvider serviceProvider) : IModelValidator<TModel>
{
    /// <inheritdoc />
    public async Task<ValidationErrorCollection> ValidateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var fluentValidator = serviceProvider.GetService<IValidator<TModel>>();

        if (fluentValidator is null)
        {
            return new ValidationErrorCollection();
        }

        var validatorResult = await fluentValidator.ValidateAsync(model, cancellationToken);

        var errors = new ValidationErrorCollection();
        if (!validatorResult.Errors.Any())
        {
            return errors;
        }

        foreach (var error in validatorResult.Errors)
        {
            errors.AddError(new ValidationError
            {
                ErrorMessage = error.ErrorMessage,
                FieldCode = error.PropertyName,
                ErrorCode = error.ErrorCode
            });
        }

        return errors;
    }

    /// <inheritdoc />
    public async Task ThrowIfInvalidAsync(TModel model, CancellationToken cancellationToken = default)
    {
        var result = await ValidateAsync(model, cancellationToken);

        if (!result.Success)
        {
            throw new ValidationException(result);
        }
    }

    /// <inheritdoc />
    public Task<ValidationErrorCollection> ValidateAsync(object model, CancellationToken cancellationToken = default)
    {
        return ValidateAsync((TModel)model, cancellationToken);
    }
}