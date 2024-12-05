using HobbyManagment.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HobbyManagment.Data.Database;
internal class HobbyConfiguration : IEntityTypeConfiguration<HobbyEntity>
{
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
