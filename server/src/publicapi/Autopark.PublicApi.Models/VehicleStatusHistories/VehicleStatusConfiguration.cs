using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.VehicleStatusHistories;

public class VehicleStatusConfiguration : IEntityTypeConfiguration<VehicleStatusHistory>
{
    public void Configure(EntityTypeBuilder<VehicleStatusHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Vehicle)
            .WithMany(x => x.VehicleStatusHistories)
            .HasForeignKey(x => x.VehicleId);
    }
}
