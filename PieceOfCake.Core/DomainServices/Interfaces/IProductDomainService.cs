using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;
using System.Collections.Generic;

namespace PieceOfCake.Core.DomainServices.Interfaces
{
    public interface IProductDomainService
    {
        Result<IReadOnlyCollection<Product>> Get();
        Result<Product> Get(long id);
        Result<Product> Create(string name);
        Result<Product> Update(long id, string name);
        Result Delete(long id);
    }
}
