using System;

namespace PieceOfCake.BlazorApp.Resources
{
    public interface ICommonTerms
    {
        public string MeasureUnit { get; }
        public string Product { get; }
        public string Dish { get; }
        string DayOfWeek(DayOfWeek dayOfWeek);
    }
}
