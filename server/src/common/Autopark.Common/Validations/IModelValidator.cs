namespace Autopark.Common.Validations;

/// <summary>
/// Интерфейс валидации модели
/// </summary>
public interface IModelValidator
{
    /// <summary>
    /// Провалидировать модель
    /// </summary>
    /// <param name="model">Объект модели</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>Результаты валидации</returns>
    Task<ValidationErrorCollection> ValidateAsync(object model, CancellationToken cancellationToken = default);
}