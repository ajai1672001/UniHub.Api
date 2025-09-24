namespace UniHub.Dto
{
    public class TenantSignupDto
    {
        public string Name { get; set; }

        public string TimeZone { get; set; }

        public AspNetUserDto AspNetUser { get; set; }
    }
}