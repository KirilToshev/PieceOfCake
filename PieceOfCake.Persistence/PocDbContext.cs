using Microsoft.EntityFrameworkCore;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

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
