using CSharpFunctionalExtensions;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.DishFeature.Entities;
using PieceOfCake.Tests.Common.Fakes.Common;
using PieceOfCake.Tests.Common.Fakes.Interfaces;
using System.Linq.Expressions;

namespace PieceOfCake.Tests.Common.Fakes;
public class MealOfTheDayTypeFakes : NameFakes<MealOfTheDayType>, IMealOfTheDayTypeFakes
{
    public MealOfTheDayTypeFakes (IResources resources, IUnitOfWork uowMock)
        : base(resources, uowMock)
    {
    }

    public MealOfTheDayType Breakfast => Create(TestsConstants.MealOfTheDayTypes.BREAKFAST);
    public MealOfTheDayType Lunch => Create(TestsConstants.MealOfTheDayTypes.LUNCH);
    public MealOfTheDayType Dinner => Create(TestsConstants.MealOfTheDayTypes.DINNER);

    public override Func<string, IResources, IUnitOfWork, CancellationToken, Task<Result<MealOfTheDayType>>> CreateFunction =>  MealOfTheDayType.Create;

    public override Expression<Func<MealOfTheDayType, string>> CacheKey => x => x.Name;
}
