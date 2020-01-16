namespace PieceOfCake.Core.Resources
{
    public interface IUserErrors
    {
        public string NameIsMandatory { get; }
        public string NameExceedsMaxLength { get; }
        public string NameAlreadyExists { get; }
    }
}
