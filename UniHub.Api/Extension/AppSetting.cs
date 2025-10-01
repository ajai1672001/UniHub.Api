namespace UniHub.Api.Extension
{
    public class AppSetting
    {
        public JWT JWT { get; set; }
    }
    public class JWT
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
