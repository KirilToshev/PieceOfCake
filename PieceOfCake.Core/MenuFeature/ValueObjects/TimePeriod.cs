using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Resources;
using System.Collections;

namespace PieceOfCake.Core.MenuFeature.ValueObjects;

public class TimePeriod : ValueObject<TimePeriod>, IEnumerable<DateOnly>
{
    private TimePeriod()
    {
    }

    private TimePeriod (DateTime startDate, DateTime endDate)
    {
        StartDate = DateOnly.FromDateTime(startDate);
        EndDate = DateOnly.FromDateTime(endDate);
    }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public int DaysDifference { get { return EndDate.DayNumber - StartDate.DayNumber + 1; } }

    public static Result<TimePeriod> Create (DateTime startDate, DateTime endDate, IResources resources)
    {
        if (startDate > endDate)
            return Result.Failure<TimePeriod>(resources.GenereteSentence(x =>
                                                x.UserErrors.PeriodStartDateLaterThanEndDate,
                                                x => startDate.ToShortDateString(), x => endDate.ToShortDateString()));

        return Result.Success(new TimePeriod(startDate, endDate.Date));
    }

    protected override bool EqualsCore (TimePeriod other)
    {
        return StartDate == other.StartDate
                && EndDate == other.EndDate;
    }

    protected override int GetHashCodeCore ()
    {
        return HashCode.Combine(StartDate, EndDate);
    }

    public IEnumerator<DateOnly> GetEnumerator ()
    {
        for (var dayIterrator = StartDate; dayIterrator <= EndDate; dayIterrator = dayIterrator.AddDays(1))
        {
            yield return dayIterrator;
        }
    }

    IEnumerator IEnumerable.GetEnumerator ()
    {
        return GetEnumerator();
    }

    public bool IsInPeriod(DateOnly date)
    {
        return date >= StartDate && date <= EndDate;
    }
}
