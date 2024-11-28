using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.Common.ValueObjects;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.DishFeature.States;

namespace PieceOfCake.DAL.EntityConfiguration;

public class DishDbConfiguration(IResources resources) : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {

        builder.ToTable("Dishes").HasKey(k => k.Id);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.FIFTY)
            .HasConversion(
                x => (string)x,
                x => Name.Create(x, resources, x => x.CommonTerms.Product, Constants.FIFTY, null).Value);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(Constants.TEN_THOUSAND);

        builder.Property(x => x.ServingSize)
            .IsRequired()
            .HasMaxLength(byte.MaxValue);

        builder.HasMany(x => x.Ingredients)
            .WithOne(x => x.Dish)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.MealOfTheDayTypes)
            .WithMany(x => x.Dishes);

        builder.HasMany(x => x.Menus)
            .WithMany(x => x.Dishes);

        builder.Property(p => p.DishState)
            .HasColumnName("State")
            .IsRequired()
            .HasConversion(
            x => x.State,
            x => StateFactory(x));
    }

    private DishState StateFactory(Core.DishFeature.Enumerations.DishState state)
    {
        switch(state)
        {
            case Core.DishFeature.Enumerations.DishState.Draft:
                return new DraftState(resources);
            case Core.DishFeature.Enumerations.DishState.AwaitingApproval:
                return new AwaitingApprovalState(resources);
            case Core.DishFeature.Enumerations.DishState.Rejected:
                return new RejectedState(resources);
            case Core.DishFeature.Enumerations.DishState.Active:
                return new ActiveState(resources);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
