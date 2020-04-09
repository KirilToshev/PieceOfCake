using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Resources;
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
                //x.Property(p => p.Name)
                //    .IsRequired()
                //    .HasMaxLength((int)Constants.FIFTY)
                //    .HasConversion(
                //        x => (string)x,
                //        x => Name.Create(x, _resources, x => x.CommonTerms.MeasureUnit, Constants.FIFTY, null).Value);
            });
        }

        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
