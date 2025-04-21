using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.OdometerHistories;

public class OdometerHistoryConfiguration : IEntityTypeConfiguration<OdometerHistory>
{
    public void Configure(EntityTypeBuilder<OdometerHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(oh => oh.Vehicle)
            .WithMany(oh => oh.OdometerHistories)
            .HasForeignKey(oh => oh.VehicleId);
    }
}
