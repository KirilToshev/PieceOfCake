using CSharpFunctionalExtensions;
using PieceOfCake.Core.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.ValueObjects
{
    public class TimePeriod : ValueObject<TimePeriod>
    {
        private TimePeriod(DateTime startDate, DateTime endDate)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public int DaysDifference { get { return (this.EndDate - this.StartDate).Days; } }

        public static Result<TimePeriod> Create(DateTime? startDate, DateTime? endDate, IResources resources)
        {
            if (!startDate.HasValue)
                return Result.Failure<TimePeriod>(resources.GenereteSentence(x =>
                                                    x.UserErrors.StartDateIsMandatory));

            if (!endDate.HasValue)
                return Result.Failure<TimePeriod>(resources.GenereteSentence(x =>
                                                    x.UserErrors.EndDateIsMandatory));

            if (startDate > endDate)
                return Result.Failure<TimePeriod>(resources.GenereteSentence(x =>
                                                    x.UserErrors.PeriodStartDateLaterThanEndDate,
                                                    x => startDate.Value.ToShortDateString(), x => endDate.Value.ToShortDateString()));

            return Result.Success(new TimePeriod(startDate.Value, endDate.Value));
        }

        protected override bool EqualsCore(TimePeriod other)
        {
            return this.StartDate == other.StartDate
                    && this.EndDate == other.EndDate;
        }

        protected override int GetHashCodeCore()
        {
            return StartDate.GetHashCode() ^ 2451 & EndDate.GetHashCode() ^ 9038;
        }
    }
}
