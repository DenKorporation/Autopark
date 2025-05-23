using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.MaintenanceRecords;

public class MaintenanceRecordConfiguration
    : IEntityTypeConfiguration<MaintenanceRecord>
{
    public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
    {
        builder
            .HasKey(mr => mr.Id);

        builder
            .Property(mr => mr.Type)
            .HasMaxLength(100);

        builder
            .Property(mr => mr.ServiceCenter)
            .HasMaxLength(255);

        builder
            .Property(mr => mr.Description)
            .HasMaxLength(255);

        builder
            .Property(mr => mr.Cost)
            .HasPrecision(11, 2);
        
        builder
            .HasOne(mr => mr.Vehicle)
            .WithMany(v => v.MaintenanceRecords)
            .HasForeignKey(mr => mr.VehicleId);
    }
}
