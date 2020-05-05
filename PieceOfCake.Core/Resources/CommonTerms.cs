using Microsoft.Extensions.Localization;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Core.Resources
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

        public string Dish => GetString(nameof(Product));

        private string GetString(string name) => _localizer[name];
    }
}
