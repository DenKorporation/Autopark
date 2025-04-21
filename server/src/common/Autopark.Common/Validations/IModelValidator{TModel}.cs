namespace Autopark.Common.Validations;

/// <summary>The ModelValidator interface.</summary>
/// <typeparam name="TModel"></typeparam>
public interface IModelValidator<in TModel> : IModelValidator
{
    /// <summary>The validate async.</summary>
    /// <param name="model">The model.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<ValidationErrorCollection> ValidateAsync(TModel model, CancellationToken cancellationToken = default);

    /// <summary>The throw if invalid async.</summary>
    /// <param name="model">The model.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task ThrowIfInvalidAsync(TModel model, CancellationToken cancellationToken = default);
}