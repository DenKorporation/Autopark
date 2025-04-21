using System.ComponentModel.Composition;

namespace Autopark.Common.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
[MetadataAttribute]
public sealed class ServiceAsInterfacesAttribute : Attribute
{
    /// <summary>
    ///     Автоматом добавляется в метаданные (<see cref="AutofacModuleBase"/>)
    /// </summary>
    public int Order { get; set; }
}
