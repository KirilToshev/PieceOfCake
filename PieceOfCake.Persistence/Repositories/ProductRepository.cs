using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(PocDbContext context) : base(context)
        {
        }
    }
}
