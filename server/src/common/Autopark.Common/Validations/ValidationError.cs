using Newtonsoft.Json;

namespace Autopark.Common.Validations;

/// <summary>
/// Класс содержащий информацию об ошибке валидации
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Код поля, в котором произошла ошибка
    /// </summary>
    public string FieldCode { get; set; }

    /// <summary>
    /// Код ошибки
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Текстовое описание ошибки для пользователя
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Строковое представление ошибки
    /// </summary>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    /// <summary>
    /// Устанавливает код поля, в котором произошла ошибка
    /// <param name="code">Код поля</param>
    /// </summary>
    public ValidationError SetField(string code)
    {
        FieldCode = code;
        return this;
    }
}
