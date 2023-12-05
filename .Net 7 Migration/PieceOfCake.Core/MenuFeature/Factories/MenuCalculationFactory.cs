using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.MenuFeature.CalculationStrategies;
using PieceOfCake.Core.MenuFeature.Enumerations;

namespace PieceOfCake.Core.MenuFeature.Factories;

internal static class MenuCalculationFactory
{
    internal static IMenuCalculationStrategy GetStrategy(MenuType type, IResources resources)
    {
        switch (type)
        {
            case MenuType.None:
                return new DefaultMenuCalculationStrategy(resources);
            case MenuType.HighProtein:
                throw new NotImplementedException();
            case MenuType.Vegetarian:
                throw new NotImplementedException();
            case MenuType.LolCarbohydrates:
                throw new NotImplementedException();
            default: 
                throw new NotImplementedException();
        }
    }
}
