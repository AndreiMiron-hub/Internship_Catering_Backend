using Assist.Lunch._4.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assist.Lunch._4.Domain.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasKey(order => order.Id);

            builder
                .Property(order => order.Number)
                .IsRequired();

            builder
                .Property(order => order.TimeSlot)
                .IsRequired();

            builder
                .Property(order => order.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(order => order.Destination)
                .WithMany(destination => destination.Orders)
                .HasForeignKey(order => order.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
