using CSharpFunctionalExtensions;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PieceOfCake.Core.DomainServices
{
    public class ProductDomainService : IProductDomainService
    {
        private readonly IResources _resources;
        private readonly IUnitOfWork _unitOfWork;

        public ProductDomainService(
            IResources resources,
            IUnitOfWork unitOfWork)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Result<IReadOnlyCollection<Product>> Get()
        {
            var productsList = _unitOfWork.ProductRepository.Get();

            return Result.Success(productsList);
        }

        public Result<Product> Get(long id)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);

            if (product == null)
                return Result.Failure<Product>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return Result.Success(product);
        }

        public Result<Product> Update(long id, string? name)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);

            if (product == null)
                return Result.Failure<Product>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return product.Update(name, _resources, _unitOfWork)
                .Tap(x => 
                {
                    _unitOfWork.ProductRepository.Update(x);
                    _unitOfWork.Save();
                });
        }

        public Result<Product> Create(string name)
        {
            return Product.Create(name, _resources, _unitOfWork)
                .Tap(x => {
                    _unitOfWork.ProductRepository.Insert(x);
                    _unitOfWork.Save(); 
                });
        }

        public Result Delete(long id)
        {
            return this.Get(id)
                .Bind(product =>
                {
                    var isProductInUse = _unitOfWork.DishRepository
                                            .Get(dish => dish.Ingredients.Any(i => i.Product.Id == product.Id))
                                            .Any();
                    if (isProductInUse)
                        return Result.Failure(_resources
                            .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Product));

                    _unitOfWork.ProductRepository.Delete(product);
                    _unitOfWork.Save();
                    return Result.Success();
                });
        }
    }
}
