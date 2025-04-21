using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autopark.PublicApi.Models.Parts;

public class PartConfiguration : IEntityTypeConfiguration<Part>
{
    public void Configure(EntityTypeBuilder<Part> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Name)
            .HasMaxLength(255);

        builder
            .Property(p => p.Category)
            .HasMaxLength(255);

        builder
            .Property(p => p.Manufacturer)
            .HasMaxLength(255);
    }
}
