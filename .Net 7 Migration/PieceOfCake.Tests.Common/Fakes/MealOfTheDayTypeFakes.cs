using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;

namespace PieceOfCake.Tests.Common.Fakes;
public class MealOfTheDayTypeFakes : NameFakes<MealOfTheDayType>
{
    public MealOfTheDayTypeFakes (IResources resources, IUnitOfWork uowMock)
        : base(resources, uowMock, MealOfTheDayType.Create)
    {
    }

    public MealOfTheDayType Breakfast => Create(TestsConstants.MealOfTheDayTypes.BREAKFAST);
    public MealOfTheDayType Lunch => Create(TestsConstants.MealOfTheDayTypes.LUNCH);
    public MealOfTheDayType Dinner => Create(TestsConstants.MealOfTheDayTypes.DINNER);
}
