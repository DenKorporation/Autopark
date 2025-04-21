using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.FuelTypes;

public class FuelTypeConfiguration : IEntityTypeConfiguration<FuelType>
{
    public void Configure(EntityTypeBuilder<FuelType> builder)
    {
        builder
            .HasKey(ft => ft.Id);

        builder
            .Property(ft => ft.Name)
            .HasMaxLength(50);
    }
}
