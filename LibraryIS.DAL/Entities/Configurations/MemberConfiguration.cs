using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryIS.DAL.Entities.Configurations
{
    internal class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            //----------Properties------------
            //Name
            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200);

            //PassportNumber
            builder
                .Property(m => m.PassportNumber)
                .IsRequired();

            //PassportSeries
            builder
                .Property(m => m.PassportSeries)
                .IsRequired();

            //DateOfBirth
            builder
                .Property(m => m.DateOfBirth)
                .IsRequired();

            //IsInBlacklist
            builder
                .Property(m => m.IsInBlacklist)
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
