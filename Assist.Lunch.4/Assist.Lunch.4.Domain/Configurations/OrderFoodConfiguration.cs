using Assist.Lunch._4.Domain.Entitites.JoiningEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assist.Lunch._4.Domain.Configurations
{
    public class OrderFoodConfiguration : IEntityTypeConfiguration<OrderFood>
    {
        public void Configure(EntityTypeBuilder<OrderFood> builder)
        {
            builder
                .HasKey(orderfood => new { orderfood.OrderId, orderfood.FoodId });

            builder
                .HasOne(orderfood => orderfood.Order)
                .WithMany(order => order.OrderFoods)
                .HasForeignKey(orderfood => orderfood.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(orderfood => orderfood.Food)
                .WithMany(food => food.OrderFoods)
                .HasForeignKey(orderfood => orderfood.FoodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
