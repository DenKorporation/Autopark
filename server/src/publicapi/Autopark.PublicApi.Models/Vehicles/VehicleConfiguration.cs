using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.Vehicles;

public class VehicleConfiguration
    : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder
            .HasKey(v => v.Id);

        builder
            .Property(v => v.Cost)
            .HasPrecision(11, 2);
        
        builder
            .HasOne(v => v.FuelType)
            .WithMany(u => u.Vehicles)
            .HasForeignKey(v => v.FuelTypeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
