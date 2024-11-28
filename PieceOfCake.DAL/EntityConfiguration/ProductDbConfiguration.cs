using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Common.ValueObjects;
using PieceOfCake.Core.IngredientFeature.Entities;

namespace PieceOfCake.DAL.EntityConfiguration;
public class ProductDbConfiguration(IResources resources) : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id); 
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Constants.FIFTY)
            .HasConversion(
                x => x.Value,
                x => Name.Create(x, resources, x => x.CommonTerms.Product, Constants.FIFTY, null).Value);
    }
}
