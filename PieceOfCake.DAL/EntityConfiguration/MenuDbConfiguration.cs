using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PieceOfCake.Core.MenuFeature.Entities;

namespace PieceOfCake.DAL.EntityConfiguration;

public class MenuDbConfiguration() : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        const string tableName = "Menus";
        builder.ToTable(tableName)
            .HasKey(k => k.Id);

        builder.Property(p => p.NumberOfPeople)
            .IsRequired();

        builder.OwnsOne(x => x.Duration, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.ToTable(tableName);
            ownedNavigationBuilder.Ignore(d => d.DaysDifference);
            ownedNavigationBuilder.Property(a => a.StartDate)
                .HasColumnName("StartDate")
                .IsRequired();
            ownedNavigationBuilder.Property(a => a.EndDate)
                .HasColumnName("EndDate")
                .IsRequired();
        });

        builder.HasMany(x => x.MealOfTheDayTypes)
            .WithMany(x => x.Menus);

        builder.HasMany(x => x.Dishes)
            .WithMany(x => x.Menus);

        builder.OwnsMany(menu => menu.Calendar, ci =>
        {
            ci.ToJson();
            ci.OwnsMany(d => d.MealOfTheDayTypes, mt =>
            {
                mt.OwnsMany(x => x.Dishes);
            });
        });
    }
}
