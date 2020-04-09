using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;

namespace PieceOfCake.Core.Entities
{
    public class Product : Entity
    {
        public Product()
        {

        }

        public string? Name { get; set; }

        //public static Result<Product> Create(string? name, IResources resources, IUnitOfWork unitOfWork)
        //{
            
        //}

        //public Result<Product> Update(string? name, IResources resources, IUnitOfWork unitOfWork)
        //{
            
        //}
    }
}
