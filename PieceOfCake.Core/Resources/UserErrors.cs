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

        public string QuantityMustBeGraterThanZero => GetString(nameof(QuantityMustBeGraterThanZero));

        public string DescriptionExceedsMaxLength => GetString(nameof(DescriptionExceedsMaxLength));

        public string DescriptionIsMandatory => GetString(nameof(DescriptionIsMandatory));

        public string InvalidStateTransition => GetString(nameof(InvalidStateTransition));

        public string IngredientAlreadyExists => GetString(nameof(IngredientAlreadyExists));

        public string ItemIsInUse => GetString(nameof(ItemIsInUse));

        public string PeriodStartDateLaterThanEndDate => GetString(nameof(PeriodStartDateLaterThanEndDate));

        public string MenuMustHaveAtLeastOneServing => GetString(nameof(MenuMustHaveAtLeastOneServing));

        public string StartDateIsMandatory => GetString(nameof(StartDateIsMandatory));

        public string EndDateIsMandatory => GetString(nameof(EndDateIsMandatory));

        public string NotEnoughDishes => GetString(nameof(NotEnoughDishes));

        private string GetString(string name) => _localizer[name];
    }
}
