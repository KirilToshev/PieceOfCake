using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.States;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace PieceOfCake.Persistence
{
    public class PocDbContext : DbContext
    {
        private readonly IResources _resources;

        public PocDbContext(
            DbContextOptions<PocDbContext> options, 
            IResources resourses)
            : base(options)
        {
            _resources = resourses;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeasureUnit>(x => 
            {
                x.ToTable(nameof(MeasureUnit) + "s").HasKey(k => k.Id);
                x.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength((int)Constants.FIFTY)
                    .HasConversion(
                        x => (string)x,
                        x => Name.Create(x, _resources, x => x.CommonTerms.MeasureUnit, Constants.FIFTY, null).Value);
            });

            modelBuilder.Entity<Product>(x =>
            {
                x.ToTable(nameof(Product) + "s").HasKey(k => k.Id);
                x.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength((int)Constants.FIFTY)
                    .HasConversion(
                        x => (string)x,
                        x => Name.Create(x, _resources, x => x.CommonTerms.Product, Constants.FIFTY, null).Value);
            });

            modelBuilder.Entity<Ingredient>(x =>
            {
                x.ToTable(nameof(Ingredient) + "s").HasKey(k => k.Id);
                x.Property(p => p.Quantity).IsRequired();
                x.HasOne(x => x.MeasureUnit)
                 .WithMany()
                 .OnDelete(DeleteBehavior.Restrict);
                x.HasOne(x => x.Product)
                 .WithMany()
                 .OnDelete(DeleteBehavior.Restrict);
                x.HasOne(x => x.Dish)
                 .WithMany(x => x.Ingredients);
            });

            modelBuilder.Entity<Dish>(x =>
            {
                x.ToTable(nameof(Dish) + "es").HasKey(k => k.Id);
                x.Property(p => p.Name)
                 .IsRequired()
                 .HasMaxLength((int)Constants.FIFTY)
                 .HasConversion(
                        x => (string)x,
                        x => Name.Create(x, _resources, x => x.CommonTerms.Product, Constants.FIFTY, null).Value);

                x.Property(x => x.Description)
                 .IsRequired()
                 .HasMaxLength((int)Constants.FIFTY_THOUSAND);

                x.HasMany(x => x.Ingredients)
                 .WithOne(x => x.Dish)
                 .OnDelete(DeleteBehavior.Cascade);

                x.HasMany(x => x.Menus)
                 .WithMany(x => x.Dishes);

                x.Property(p => p.DishState)
                 .HasColumnName("State")
                 .IsRequired()
                 .HasConversion(
                    x => x.State,
                    x => StateConversion(x));
            });

            modelBuilder.Entity<Menu>(x =>
            {
                x.ToTable(nameof(Menu) + "s").HasKey(k => k.Id);
                x.Property(p => p.ServingsPerDay)
                 .IsRequired();

                x.HasMany(x => x.Dishes)
                 .WithMany(x => x.Menus);

                x.Property(p => p.StartDate)
                 .IsRequired();

                x.Property(p => p.EndDate)
                 .IsRequired();
            });
        }

        private DishState StateConversion(Core.Enumerations.DishState state)
        {
            switch (state)
            {
                case Core.Enumerations.DishState.Draft:
                    return new DraftState(_resources);
                case Core.Enumerations.DishState.AwaitingApproval:
                    return new AwaitingApprovalState(_resources);
                case Core.Enumerations.DishState.Rejected:
                    return new RejectedState(_resources);
                case Core.Enumerations.DishState.Active:
                    return new ActiveState(_resources);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Menu> Menus { get; set; }
    }
}
