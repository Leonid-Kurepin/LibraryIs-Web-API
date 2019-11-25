using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryIS.DAL.Entities.Configurations
{
    internal class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            //----------Properties------------
            //Name
            builder
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}