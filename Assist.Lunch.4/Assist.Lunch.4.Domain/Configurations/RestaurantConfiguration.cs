using Assist.Lunch._4.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assist.Lunch._4.Domain.Configurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder
                .HasKey(restaurant => restaurant.Id);

            builder
                .Property(restaurant => restaurant.Name)
                .HasMaxLength(60)
                .IsRequired();

            builder
                .Property(restaurant => restaurant.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(restaurant => restaurant.IsAvailable)
                .IsRequired();
        }
    }
}
