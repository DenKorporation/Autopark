using Autopark.Common.Domain;
using Autopark.PublicApi.Models.PartReplacements;

namespace Autopark.PublicApi.Models.Parts;

public class Part : EntityBase
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Manufacturer { get; set; }
    public uint ServiceLife{ get; set; }
    public ICollection<PartReplacement> PartReplacements { get; set; }
}
