using PieceOfCake.Core.Enumerations;

namespace PieceOfCake.Core.Resources
{
    public interface ICommonTerms
    {
        public string MeasureUnit { get; }
        public string Product { get; }
        public string Dish { get; }

        string DishState(DishState state);
    }
}
