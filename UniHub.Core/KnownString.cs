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
        public static class EmailPlaceholders
        {
            public const string UserName = "[UserName]";
            public const string AppName = "[AppName]";
            public const string Otp = "[Otp]";
            public const string OtpExpiryMinutes = "[OtpExpiryMinutes]";
            public const string VerificationCode = "[VerificationCode]";
            public const string CodeExpiryMinutes = "[CodeExpiryMinutes]";
            public const string CopyRightYear = "[CopyRightYear]";
            public const string SupportEmail = "[SupportEmail]";
        }

    }
}