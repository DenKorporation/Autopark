using System.Collections;
using Newtonsoft.Json;

namespace Autopark.Common.Validations;

/// <summary>
/// Класс содержащий список ошибок валидации
/// </summary>
public class ValidationErrorCollection : IEnumerable<ValidationError>
{
    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public ValidationErrorCollection()
    {
    }

    /// <summary>
    /// Конструктор, принимающий список ошибок
    /// <param name="errors">список ошибок</param>
    /// </summary>
    public ValidationErrorCollection(params ValidationError[] errors)
    {
        Errors.AddRange(errors);
    }

    /// <summary>
    /// Конструктор, принимающий список ошибок
    /// <param name="errors">список ошибок</param>
    /// </summary>
    public ValidationErrorCollection(IEnumerable<ValidationError> errors)
    {
        Errors.AddRange(errors);
    }

    /// <summary>
    /// Список ошибок
    /// </summary>
    public List<ValidationError> Errors { get; set; } = new List<ValidationError>();

    /// <summary>
    /// Есть ли ошибки валидации
    /// </summary>
    public bool Success => Errors.Count == 0;

    /// <summary>
    /// Добавляет ошибку валидации, если её ещё нет
    /// <param name="error">Ошибка валидации</param>
    /// </summary>
    public void AddError(ValidationError error)
    {
        if (!Errors.Any(x => x.FieldCode == error.FieldCode && x.ErrorCode == error.ErrorCode && x.ErrorMessage == error.ErrorMessage))
        {
            Errors.Add(error);
        }
    }

    /// <inheritdoc />
    public IEnumerator<ValidationError> GetEnumerator()
    {
        return Errors.GetEnumerator();
    }

    /// <summary>
    /// Возвращает строковое представление ошибки
    /// </summary>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(Errors);
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return Errors.GetEnumerator();
    }
}