using CSharpFunctionalExtensions;
using PieceOfCake.Core.BusinessRules;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PieceOfCake.BusinessRules
{
    public class ProductBusinessRules : IProductBusinessRules
    {
        private readonly IResources _resources;
        private readonly IUnitOfWork _unitOfWork;

        public ProductBusinessRules(
            IResources resources,
            IUnitOfWork unitOfWork)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Result<IReadOnlyCollection<Product>> Get()
        {
            var measureUnits = _unitOfWork.ProductRepository.Get();

            if (!measureUnits.Any())
                return Result.Failure<IReadOnlyCollection<Product>>(
                    _resources.GenereteSentence(x => x.UserErrors.SequenceContainsNoElements));

            return Result.Success(measureUnits);
        }

        public Result<Product> Get(long id)
        {
            var measureUnit = _unitOfWork.ProductRepository.GetById(id);

            if (measureUnit == null)
                return Result.Failure<Product>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return Result.Success(measureUnit);
        }

        public Result<Product> Update(long id, string? name)
        {
            var measureUnit = _unitOfWork.ProductRepository.GetById(id);

            if (measureUnit == null)
                return Result.Failure<Product>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return measureUnit.Update(name, _resources, _unitOfWork)
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
                .OnFailure(() => _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()))
                .Tap(mu => {
                    _unitOfWork.ProductRepository.Delete(mu);
                    _unitOfWork.Save();
                });
        }
    }
}
