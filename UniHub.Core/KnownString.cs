namespace UniHub.Core
{
    public static class KnownString
    {
        public static class Schema
        {
            public const string Default = "dbo";
            public const string Tenant = "tenant";
            public const string Identity = "identity";
            public const string Email = "email";
        }

        public static class Headers
        {
            public static string Apikey = "x-api-key";
            public static string Tenant = "x-tenant-id";
            public static string Authorization = "Authorization";
        }
    }
}