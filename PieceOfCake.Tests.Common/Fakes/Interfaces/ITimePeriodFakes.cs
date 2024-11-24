using PieceOfCake.Core.MenuFeature.ValueObjects;

namespace PieceOfCake.Tests.Common.Fakes.Interfaces;
public interface ITimePeriodFakes
{
    TimePeriod FiveDays { get; }
    TimePeriod FourDays { get; }
    TimePeriod OneDay { get; }
    TimePeriod ThreeDays { get; }
    TimePeriod TwoDays { get; }
    TimePeriod Week { get; }

    TimePeriod Create (int daysDifference);
}
