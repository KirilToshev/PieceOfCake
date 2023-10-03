using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.MenuFeature.ValueObjects;
using PieceOfCake.Tests.Common.Fakes.Interfaces;

namespace PieceOfCake.Tests.Common.Fakes;

public class TimePeriodFakes : ITimePeriodFakes
{
    private readonly IResources _resources;

    public TimePeriodFakes (IResources resources)
    {
        _resources = resources;
    }
    public TimePeriod OneDay => Create(0);
    public TimePeriod TwoDays => Create(1);
    public TimePeriod ThreeDays => Create(2);
    public TimePeriod FourDays => Create(3);
    public TimePeriod FiveDays => Create(4);
    public TimePeriod Week => Create(6);

    public TimePeriod Create (int daysDifference)
    {
        return TimePeriod.Create(DateTime.Now, DateTime.Now.AddDays(daysDifference), _resources).Value;
    }
}
