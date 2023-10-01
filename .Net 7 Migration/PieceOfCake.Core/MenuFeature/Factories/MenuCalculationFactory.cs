using PieceOfCake.Core.MenuFeature.CalculationStrategies;
using PieceOfCake.Core.MenuFeature.Enumerations;

namespace PieceOfCake.Core.MenuFeature.Factories;

internal static class MenuCalculationFactory
{
    internal static IMenuCalculationStrategy Create(MenuType type)
    {
        switch (type)
        {
            case MenuType.None:
                return new DefaultMenuCalculationStrategy();
            case MenuType.HighProtein:
                throw new NotImplementedException();
                break;
            case MenuType.Vegetarian:
                throw new NotImplementedException();
                break;
            case MenuType.LolCarbohydrates:
                throw new NotImplementedException();
                break;
            default: 
                throw new NotImplementedException();
        }
    }
}
