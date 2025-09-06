namespace UniHub.Domain.Entities
{
    public class Tenant : BaseSoftDeleteIdEntity<Guid>
    {
        public string Name { get; set; }
        public string TimeZone { get; set; }
    }
}