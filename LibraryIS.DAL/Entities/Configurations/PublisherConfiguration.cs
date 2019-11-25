using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryIS.DAL.Entities.Configurations
{
    internal class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            //----------Properties------------
            //Name
            builder
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}