using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Entities.EFCoreShortcomings;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PieceOfCake.Core.Entities
{
    public class Menu : Entity
    {
        #pragma warning disable 8618
        #warning Sparation Of Concerns violation
        //This is required to suppress warnings/errors in the default(empty) constructor
        //required by Moq and EF Core to construct this object in the UnitTests
        protected Menu()
        {
        }

        private Menu(TimePeriod duration, byte servingsPerDay)
        {
            this.StartDate = duration.StartDate;
            this.EndDate = duration.EndDate;
            this.ServingsPerDay = servingsPerDay;
            this.Dishes = new HashSet<DishMenu>();
        }

        public byte ServingsPerDay { get; private set; }

        public Result<TimePeriod> CalculateDuration(IResources resources)
        {
            return TimePeriod.Create(this.StartDate, this.EndDate, resources);
        }

        //NOTE: Start and End Dates only exist because EF Core is not able to map one field to multiple columns
        // https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions
        //If this limitation is fixed refactor towards one property - Duration of type TimePeriod
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public virtual ICollection<DishMenu> Dishes { get; protected set; }

        public static Result<Menu> Create(DateTime? startDate, DateTime? endDate, byte servingsPerDay, IResources resources)
        {
            var duration = TimePeriod.Create(startDate, endDate, resources);
            if (duration.IsFailure)
                return duration.ConvertFailure<Menu>();

            if (servingsPerDay == 0)
                return Result.Failure<Menu>(resources.GenereteSentence(x => x.UserErrors.MenuMustHaveAtLeastOneServing));

            return Result.Success(new Menu(duration.Value, servingsPerDay));
        }

        public Result<Menu> Update(DateTime? startDate, DateTime? endDate, byte servingsPerDay, IResources resources)
        {
            var menuResult = Create(startDate, endDate, servingsPerDay, resources);
            if (menuResult.IsFailure)
                return menuResult;

            this.ServingsPerDay = servingsPerDay;
            this.StartDate = startDate!.Value;
            this.EndDate = endDate!.Value;
            
            return Result.Success(this);
        }

        public Result GenerateDishesList(IUnitOfWork unitOfWork, IResources resources)
        {
            var durationResult = CalculateDuration(resources);
            if (durationResult.IsFailure)
                return durationResult.ConvertFailure<IEnumerable<Dish>>();

            var totalNumberOfServings = ServingsPerDay * durationResult.Value.DaysDifference;
            var dishesList = unitOfWork.DishRepository.Get();

            if (dishesList.Count() < totalNumberOfServings)
                return Result.Failure<IEnumerable<Dish>>(resources.GenereteSentence(x => x.UserErrors.NotEnoughDishes));

            //var result = dishesList.ToList();
            //for (int i = 0; i < totalNumberOfServings - dishesList.Count(); i++)
            //{
            //    result.Add(dishesList.ElementAt(i % dishesList.Count()));
            //}

            this.Dishes = dishesList.Take(totalNumberOfServings)
                .Select(x => new DishMenu()
                {
                    DishId = x.Id,
                    MenuId = Id
                })
                .ToList();

            return Result.Success();
        }

        public void ClearAllRelatedDishes()
        {
            this.Dishes.Clear();
        }
    }
}
