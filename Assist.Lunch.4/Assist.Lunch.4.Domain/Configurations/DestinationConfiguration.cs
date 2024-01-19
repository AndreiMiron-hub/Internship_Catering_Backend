using Assist.Lunch._4.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assist.Lunch._4.Domain.Configurations
{
    public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> builder)
        {
            builder
                .HasKey(destination => destination.Id);

            builder
                .Property(destination => destination.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder
                .Property(destination => destination.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(destination => destination.Address)
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
