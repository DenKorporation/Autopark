using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.EventHistories;

public class EventHistoryConfiguration : IEntityTypeConfiguration<EventHistory>
{
    public void Configure(EntityTypeBuilder<EventHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Description)
            .HasMaxLength(255);

        builder
            .HasOne(x => x.Vehicle)
            .WithMany(x => x.EventHistories)
            .HasForeignKey(x => x.VehicleId);

        builder
            .HasOne(x => x.EventType)
            .WithMany(x => x.EventHistories)
            .HasForeignKey(x => x.EventTypeId);
    }
}
