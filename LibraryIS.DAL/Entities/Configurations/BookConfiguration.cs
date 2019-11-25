using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryIS.DAL.Entities.Configurations
{
    internal class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            //----------Properties------------
            //Name
            builder
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(255);

            //AuthorId
            builder
                .Property(b => b.AuthorId)
                .IsRequired(false);

            //PublisherId
            builder
                .Property(b => b.PublisherId)
                .IsRequired(false);

            //ISBN
            builder
                .Property(b => b.Isbn)
                .IsRequired(false)
                .HasMaxLength(13);

            //Summary
            builder
                .Property(b => b.Summary)
                .IsRequired(false)
                .HasMaxLength(500);

            //CountAvailable
            builder
                .Property(b => b.CountAvailable)
                .IsRequired()
                .HasDefaultValue(1);

            //----------Relations------------
            //Author
            builder.HasOne<Author>(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            //Publisher
            builder.HasOne<Publisher>(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}