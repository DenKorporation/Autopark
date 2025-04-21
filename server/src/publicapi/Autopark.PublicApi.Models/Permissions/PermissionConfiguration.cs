using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.Permissions;

public class PermissionConfiguration
    : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .HasIndex(p => p.Number)
            .IsUnique();

        builder
            .Property(p => p.Number)
            .HasMaxLength(9);
        
        builder
            .HasOne(v => v.Vehicle)
            .WithMany(p => p.Permissions)
            .HasForeignKey(p => p.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
