namespace Autopark.Common.Attributes;

/// <summary>
/// Регистрируем сервиc в di как самого себя
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ServiceAsSelfAttribute : Attribute
{
}
