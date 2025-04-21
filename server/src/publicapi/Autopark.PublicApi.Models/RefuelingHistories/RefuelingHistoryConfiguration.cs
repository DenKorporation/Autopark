using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.RefuelingHistories;

public class RefuelingHistoryConfiguration : IEntityTypeConfiguration<RefuelingHistory>
{
    public void Configure(EntityTypeBuilder<RefuelingHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Vehicle)
            .WithMany(x => x.RefuelingHistories)
            .HasForeignKey(x => x.VehicleId);
    }
}
