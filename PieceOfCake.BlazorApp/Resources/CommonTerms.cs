using Microsoft.Extensions.Localization;
using System;

namespace PieceOfCake.BlazorApp.Resources
{
    public class CommonTerms : ICommonTerms
    {
        private readonly IStringLocalizer<CommonTerms> _localizer;

        public CommonTerms(IStringLocalizer<CommonTerms> localizer)
        {
            _localizer = localizer;
        }

        public string MeasureUnit => GetString(nameof(MeasureUnit));

        public string Product => GetString(nameof(Product));

        public string Dish => GetString(nameof(Dish));

        public string DayOfWeek(DayOfWeek dayOfWeek)
        {
            return GetString(dayOfWeek.ToString());
        }

        private string GetString(string name) => _localizer[name];
    }
}
