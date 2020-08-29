namespace PieceOfCake.Core.Resources
{
    public interface IUserErrors
    {
        public string NameIsMandatory { get; }
        public string NameExceedsMaxLength { get; }
        public string NameBelowMinLength { get; }
        public string NameAlreadyExists { get; }
        public string SequenceContainsNoElements { get; }
        public string IdNotFound { get; }
        public string QuantityMustBeGraterThanZero { get; }
        public string DescriptionExceedsMaxLength { get; }
        public string DescriptionIsMandatory { get; }
        public string InvalidStateTransition { get; }
        public string IngredientAlreadyExists { get; }
        public string ItemIsInUse { get; }
    }
}
