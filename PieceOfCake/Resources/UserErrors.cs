using Microsoft.Extensions.Localization;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Api.Resources
{
    public class UserErrors : IUserErrors
    {
        private readonly IStringLocalizer<UserErrors> _localizer;

        public UserErrors(IStringLocalizer<UserErrors> localizer)
        {
            _localizer = localizer;
        }

        public string NameIsMandatory { get => GetString(nameof(NameIsMandatory)); }

        public string NameExceedsMaxLength { get => GetString(nameof(NameExceedsMaxLength)); }

        private string GetString(string name) => _localizer[name];
    }
}
