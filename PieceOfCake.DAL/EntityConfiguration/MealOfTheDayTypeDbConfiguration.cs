using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Common.ValueObjects;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.DAL.EntityConfiguration;
public class MealOfTheDayTypeDbConfiguration(IResources resources) 
    : IEntityTypeConfiguration<MealOfTheDayType>
{
    public void Configure(EntityTypeBuilder<MealOfTheDayType> builder)
    {
        builder.ToTable("MealOfTheDayTypes");
        builder.HasKey(x => x.Id); 
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(Constants.FIFTY)
            .HasConversion(
                x => x.Value,
                x => Name.Create(x, resources, x => x.CommonTerms.MealOfTheDayType, Constants.FIFTY, null).Value);
    }
}
