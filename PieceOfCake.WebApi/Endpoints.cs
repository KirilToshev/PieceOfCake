namespace PieceOfCake.WebApi;

public static class Endpoints
{
    public static class Dishes
    {
        private const string Base = "/dishes";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
        public const string Delete = Base;
        public const string Update = Base;
    }
}
