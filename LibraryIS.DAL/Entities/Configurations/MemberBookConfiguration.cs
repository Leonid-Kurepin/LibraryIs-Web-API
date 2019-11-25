using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryIS.DAL.Entities.Configurations
{
    internal class MemberBookConfiguration : IEntityTypeConfiguration<MemberBook>
    {
        public void Configure(EntityTypeBuilder<MemberBook> builder)
        {
            //----------Keys------------
            //Name
            builder
                .HasKey(mb => new {mb.MemberId, mb.BookId});

            //----------Relations------------
            builder
                .HasOne(mb => mb.Member)
                .WithMany(m => m.MemberBooks)
                .HasForeignKey(mb => mb.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(mb => mb.Book)
                .WithMany(b => b.MemberBooks)
                .HasForeignKey(mb => mb.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
