using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PieceOfCake.Core.IngredientFeature.ValueObjects;

namespace PieceOfCake.DAL.EntityConfiguration;

public class IngredientConfiguration() 
    : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.ToTable("Ingredients")
            .HasKey(k => k.Id);
        builder.Property(k => k.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.Quantity).IsRequired();
        builder.HasOne(x => x.MeasureUnit)
         .WithMany()
         .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Product)
         .WithMany()
         .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Dish)
         .WithMany(x => x.Ingredients);
    }
}
