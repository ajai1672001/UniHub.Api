using UniHub.Domain.Interface;

namespace UniHub.Domain.Entities.Identity
{
    public class UserOtp : BaseSoftDeleteIdEntity<Guid>, IHaveUserIdEntityService
    {
        public string Otp { get; set; }
        public Guid AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }
    }
}