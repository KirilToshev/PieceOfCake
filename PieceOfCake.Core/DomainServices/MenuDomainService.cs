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
            var menuList = _unitOfWork.MenuRepository.Get();

            return Result.Success(menuList);
        }

        public Result<Menu> Get(long id)
        {
            var menu = _unitOfWork.MenuRepository.GetById(id);

            if (menu == null)
                return Result.Failure<Menu>(
                    _resources.GenereteSentence(x => x.UserErrors.IdNotFound, x => id.ToString()));

            return Result.Success(menu);
        }

        public Result<Menu> Update(long id, DateTime? startDate, DateTime? endDate, byte servingsPerDay)
        {
            var menuResult = this.Get(id);
            if (menuResult.IsFailure)
                return menuResult;

            var previousPeriodDaysCount = menuResult.Value.CalculateDuration(_resources);
            if (previousPeriodDaysCount.IsFailure)
                return previousPeriodDaysCount.ConvertFailure<Menu>();

            var updateResult = menuResult.Value.Update(startDate, endDate, servingsPerDay, _resources);
            if (updateResult.IsFailure)
                return updateResult;

            var currentPeriodDaysCount = updateResult.Value.CalculateDuration(_resources);
            if (currentPeriodDaysCount.IsFailure)
                return currentPeriodDaysCount.ConvertFailure<Menu>();

            menuResult.Value.ClearAllRelatedDishes();
            
            _unitOfWork.MenuRepository.Update(menuResult.Value);
            _unitOfWork.Save();

            return Result.Success(menuResult.Value);
        }

        public Result<Menu> Create(DateTime? startDate, DateTime? endDate, byte servingsPerDay)
        {
            return Menu.Create(startDate, endDate, servingsPerDay, _resources)
                .Tap(menu =>
                {
                    _unitOfWork.MenuRepository.Insert(menu);
                    _unitOfWork.Save();
                });
        }

        public Result Delete(long id)
        {
            return this.Get(id)
                .Tap(menu =>
                {
                    _unitOfWork.MenuRepository.Delete(menu);
                    _unitOfWork.Save();
                });
        }

        public Result<Menu> GenerateDishesList(long id)
        {
            var menuResult = this.Get(id);
            if (menuResult.IsFailure)
                return menuResult;

            menuResult.Value.ClearAllRelatedDishes();
            _unitOfWork.MenuRepository.Update(menuResult.Value);
            _unitOfWork.Save();

            var dishesListResult = menuResult.Value.GenerateDishesList(_unitOfWork, _resources);
            if (dishesListResult.IsFailure)
                return dishesListResult.ConvertFailure<Menu>();

            _unitOfWork.MenuRepository.Update(menuResult.Value);
            _unitOfWork.Save();

            return Result.Success(menuResult.Value);
        }
    }
}
