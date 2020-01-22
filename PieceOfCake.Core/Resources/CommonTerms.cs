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

        public string MeasureUnit { get => GetString(nameof(MeasureUnit)); }

        private string GetString(string name) => _localizer[name];
    }
}
