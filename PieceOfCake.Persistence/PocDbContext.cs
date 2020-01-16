using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace PieceOfCake.Persistence
{
    public class PocDbContext : DbContext
    {
        public PocDbContext(DbContextOptions<PocDbContext> options)
            : base(options)
        {
        }

        public DbSet<MeasureUnit> MeasureUnits { get; set; }
    }
}
