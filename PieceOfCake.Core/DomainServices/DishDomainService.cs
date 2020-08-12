using CSharpFunctionalExtensions;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PieceOfCake.Core.DomainServices
{
    public class DishDomainService : IDishDomainService
    {
        private readonly IResources _resources;
        private readonly IUnitOfWork _unitOfWork;

        public DishDomainService(
            IResources resources,
            IUnitOfWork unitOfWork)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }


        public Result<IReadOnlyCollection<Dish>> Get()
        {
            var dishes = _unitOfWork.DishRepository.Get();

            if (!dishes.Any())
            {
                return Result.Failure<IReadOnlyCollection<Dish>>(
                    _resources.GenereteSentence(x => x.UserErrors.SequenceContainsNoElements));
            }

            return Result.Success(dishes);
        }

        public Result<Dish> Get(long id)
        {
            var dish = _unitOfWork.DishRepository.GetById(id);

            if (dish == null)
                return Result.Failure<Dish>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return Result.Success(dish);
        }

        public Result<Dish> UpdateNameAndDescritption(long id, string name, string description)
        {
            var dishResult = this.Get(id);
            if (dishResult.IsFailure)
                return dishResult;

            return dishResult.Value.UpdateNameAndDescritption(name, description, _resources)
                .Tap(dish =>
                {
                    _unitOfWork.DishRepository.Update(dish);
                    _unitOfWork.Save();
                });
        }

        public Result<Dish> Create(string name, string description)
        {
            return Dish.Create(name, description, _resources)
                .Tap(dish =>
                {
                    _unitOfWork.DishRepository.Insert(dish);
                    _unitOfWork.Save();
                });
        }

        public Result Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}
