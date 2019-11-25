using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryIS.DAL.Entities.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //----------Properties------------
            //Name
            builder
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(200);

            //Email
            builder
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            //Password
            builder
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            //Role
            builder
                .Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
