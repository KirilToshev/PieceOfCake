namespace PieceOfCake.WebApi;

public static class Endpoints
{
    private const string GUID_ID = "{id:guid}";

    public static class Dishes
    {
        private const string Base = "/dishes";
        public const string Create = Base;
        public const string Get = $"{Base}/{GUID_ID}";
        public const string GetAll = Base;
        public const string Delete = $"{Base}/{GUID_ID}";
        public const string Update = $"{Base}/{GUID_ID}";
    }

    public static class Menus
    {
        private const string Base = "/menus";
        public const string Create = Base;
        public const string Get = $"{Base}/{GUID_ID}";
        public const string GetAll = Base;
        public const string Delete = $"{Base}/{GUID_ID}";
        public const string Update = $"{Base}/{GUID_ID}";
        public const string GenerateDishesList = $"{Base}";
    }
}
