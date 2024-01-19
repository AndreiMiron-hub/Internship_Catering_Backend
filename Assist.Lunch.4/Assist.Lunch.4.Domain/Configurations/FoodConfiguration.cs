using Assist.Lunch._4.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assist.Lunch._4.Domain.Configurations
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder
                .HasKey(food => food.Id);

            builder
                .Property(food => food.Name)
                .HasMaxLength(60)
                .IsRequired();

            builder
                .Property(food => food.Category)
                .IsRequired();

            builder
                .Property(food => food.Price)
                .IsRequired();

            builder
                .Property(food => food.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .HasOne(food => food.Restaurant)
                .WithMany(restaurant => restaurant.Foods)
                .HasForeignKey(food => food.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
