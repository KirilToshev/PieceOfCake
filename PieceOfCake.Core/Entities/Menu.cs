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
        private readonly IResources _resources;

        #pragma warning disable 8618
        #warning Sparation Of Concerns violation
        //This is required to suppress warnings/errors in the default(empty) constructor
        //required by Moq and EF Core to construct this object in the UnitTests
        protected Menu()
        {
        }

        private Menu(TimePeriod duration, byte servingsPerDay, IResources resources)
        {
            this.StartDate = duration.StartDate;
            this.EndDate = duration.EndDate;
            this.ServingsPerDay = servingsPerDay;
            this._resources = resources;
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

        public static Result<Menu> Create(TimePeriod duration, byte servingsPerDay, IResources resources)
        {
            if (servingsPerDay == 0)
                return Result.Failure<Menu>(resources.GenereteSentence(x => x.UserErrors.MenuMustHaveAtLeastOneServing));

            return Result.Success(new Menu(duration, servingsPerDay, resources));
        }

        public Result<IEnumerable<Dish>> GenerateDishesList(IUnitOfWork unitOfWork)
        {
            var durationResult = CalculateDuration(_resources);
            if (durationResult.IsFailure)
                return durationResult.ConvertFailure<IEnumerable<Dish>>();

            var totalNumberOfServings = ServingsPerDay * durationResult.Value.DaysDifference;
            var dishesList = unitOfWork.DishRepository.Get();

            if (dishesList.Count() >= totalNumberOfServings)
                return Result.Success(dishesList.Take(totalNumberOfServings));

            var result = dishesList.ToList();
            for (int i = 0; i < totalNumberOfServings - dishesList.Count(); i++)
            {
                result.Add(dishesList.ElementAt(i % dishesList.Count()));
            }

            return Result.Success(result.AsEnumerable());
        }
    }
}
