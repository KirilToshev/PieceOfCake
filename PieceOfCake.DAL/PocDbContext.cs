using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;
using PieceOfCake.Core.MenuFeature.Entities;
using PieceOfCake.DAL.EntityConfiguration;

namespace PieceOfCake.DAL;

public class PocDbContext(DbContextOptions<PocDbContext> options, IResources resources) : DbContext(options)
{
    public DbSet<MealOfTheDayType> MealOfTheDayTypes { get; set; }
    public DbSet<MeasureUnit> MeasureUnits { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Menu> Menus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new MealOfTheDayTypeDbConfiguration(resources)
            .Configure(modelBuilder.Entity<MealOfTheDayType>());
        new ProductDbConfiguration(resources)
            .Configure(modelBuilder.Entity<Product>());
        new MeasureUnitDbConfiguration(resources)
            .Configure(modelBuilder.Entity<MeasureUnit>());
        new IngredientDbConfiguration()
            .Configure(modelBuilder.Entity<Ingredient>());
        new DishDbConfiguration(resources)
            .Configure(modelBuilder.Entity<Dish>());
        new MenuDbConfiguration()
            .Configure(modelBuilder.Entity<Menu>());
    }
}
