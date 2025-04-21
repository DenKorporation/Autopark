using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.PartReplacements;

public class PartReplacementConfiguration : IEntityTypeConfiguration<PartReplacement>
{
    public void Configure(EntityTypeBuilder<PartReplacement> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .HasOne(pr => pr.Part)
            .WithMany(p => p.PartReplacements)
            .HasForeignKey(pr => pr.PartId);

        builder
            .HasOne(pr => pr.Vehicle)
            .WithMany(p => p.PartReplacements)
            .HasForeignKey(pr => pr.VehicleId);
    }
}
