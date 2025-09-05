namespace UniHub.Entities
{
    public class AspNetUserRefershToken : BaseSoftDeleteIdEntity<Guid>
    {
        public string AccessToken { get; set; }

        public Guid UserId { get; set; }

        public string RefershToken { get; set; }

        public DateTime RefershTokenExpires { get; set; }

        public DateTime GenerateAt { get; set; }
    }
}