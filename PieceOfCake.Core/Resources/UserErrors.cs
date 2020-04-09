using Microsoft.Extensions.Localization;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.Core.Resources
{
    public class UserErrors : IUserErrors
    {
        private readonly IStringLocalizer<UserErrors> _localizer;

        public UserErrors(IStringLocalizer<UserErrors> localizer)
        {
            _localizer = localizer;
        }

        public string NameIsMandatory => GetString(nameof(NameIsMandatory));

        public string NameExceedsMaxLength => GetString(nameof(NameExceedsMaxLength));

        public string NameAlreadyExists => GetString(nameof(NameAlreadyExists));

        public string SequenceContainsNoElements => GetString(nameof(SequenceContainsNoElements));

        public string IdNotFound => GetString(nameof(IdNotFound));

        public string NameBelowMinLength => GetString(nameof(NameBelowMinLength));

        private string GetString(string name) => _localizer[name];
    }
}
