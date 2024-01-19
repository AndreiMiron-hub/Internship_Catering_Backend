using Assist.Lunch._4.Domain.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assist.Lunch._4.Domain.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(user => user.Id);

            builder
                .Property(user => user.FirstName)
                .HasMaxLength(30)
                .IsRequired();

            builder
                .Property(user => user.LastName)
                .HasMaxLength(30)
                .IsRequired();

            builder
                .Property(user => user.Email)
                .HasMaxLength(62)
                .IsRequired();

            builder
                .Property(user => user.Password)
                .HasMaxLength(60)
                .IsRequired();

            builder
                .Property(user => user.IsAdmin)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .Property(user => user.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}
