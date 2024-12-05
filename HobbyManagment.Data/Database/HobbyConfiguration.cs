using HobbyManagment.Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HobbyManagment.Data.Database;

/// <summary>
/// Configuration class for hobby entities. 
/// </summary>
internal class HobbyConfiguration : IEntityTypeConfiguration<HobbyEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<HobbyEntity> builder)
    {
        builder
            .HasKey(x => x.HobbyId);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();

        builder
            .Property(x => x.Name)
            .HasMaxLength(50);
    }
}
