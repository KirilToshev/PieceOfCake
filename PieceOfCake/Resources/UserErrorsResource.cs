using Microsoft.Extensions.Localization;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Resources
{
    public class UserErrorsResource : IUserErrorsResource
    {
        private readonly IStringLocalizer<UserErrorsResource> _localizer;

        public UserErrorsResource(IStringLocalizer<UserErrorsResource> localizer)
        {
            _localizer = localizer;
        }

        public string NameIsMandatory { get => GetString(nameof(NameIsMandatory)); }

        private string GetString(string name) => _localizer[name];
    }
}
