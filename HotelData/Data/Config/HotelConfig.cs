using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelData.Data.Config
{
    public class HotelConfig : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.ToTable("Hotels");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.ExternalId).HasColumnType("integer").IsRequired();
            builder.Property(n => n.Name).IsRequired().HasMaxLength(500);
            builder.Property(n => n.Region).HasMaxLength(250).IsRequired(false);
            builder.Property(n => n.Country).HasMaxLength(250).IsRequired(false);
            builder.Property(n => n.City).HasMaxLength(250).IsRequired(false);
            builder.Property(n => n.Latitude).HasColumnType("float").IsRequired(false);
            builder.Property(n => n.Longitude).HasColumnType("float").IsRequired(false);
            builder.Property(n => n.SupplierId).HasColumnType("integer").IsRequired(true);
            builder.Property(n => n.SupplierName).HasMaxLength(250).IsRequired(false);
        }
    }
}
