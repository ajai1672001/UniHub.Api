namespace UniHub.Entities
{
    public class UserOtp : BaseSoftDeleteIdEntity<Guid>
    {
        public string Otp { get; set; }
        public Guid UserId { get; set; }
        public bool IsUsed { get; set; }
    }
}