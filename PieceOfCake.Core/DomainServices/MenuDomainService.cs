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
    public class MenuDomainService : IMenuDomainService
    {
        private readonly IResources _resources;
        private readonly IUnitOfWork _unitOfWork;

        public MenuDomainService(
            IResources resources,
            IUnitOfWork unitOfWork)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Result<IReadOnlyCollection<Menu>> Get()
        {
            throw new NotImplementedException();
            //var productsList = _unitOfWork.ProductRepository.Get();

            //return Result.Success(productsList);
        }

        public Result<Menu> Get(long id)
        {
            throw new NotImplementedException();
            //var product = _unitOfWork.ProductRepository.GetById(id);

            //if (product == null)
            //    return Result.Failure<Product>(
            //        _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            //return Result.Success(product);
        }

        public Result<Menu> Update(DateTime? startDate, DateTime? endDate, byte servingsPerDay)
        {
            throw new NotImplementedException();
        }

        public Result<Menu> Create(DateTime? startDate, DateTime? endDate, byte servingsPerDay)
        {
            throw new NotImplementedException();
        }

        public Result Delete(long id)
        {
            throw new NotImplementedException();
            //return this.Get(id)
            //    .Bind(product =>
            //    {
            //        var isProductInUse = _unitOfWork.DishRepository
            //                                .Get(dish => dish.Ingredients.Any(i => i.Product.Id == product.Id))
            //                                .Any();
            //        if (isProductInUse)
            //            return Result.Failure(_resources
            //                .GenereteSentence(x => x.UserErrors.ItemIsInUse, x => x.CommonTerms.Product));

            //        _unitOfWork.ProductRepository.Delete(product);
            //        _unitOfWork.Save();
            //        return Result.Success();
            //    });
        }
    }
}
