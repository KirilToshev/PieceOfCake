namespace PieceOfCake.Core.Resources
{
    public interface IUserErrors
    {
        public string NameIsMandatory { get; }
        public string NameExceedsMaxLength { get; }
        public string NameAlreadyExists { get; }
        public string SequenceContainsNoElements { get; }
        public string IdNotFound { get; }
    }
}
