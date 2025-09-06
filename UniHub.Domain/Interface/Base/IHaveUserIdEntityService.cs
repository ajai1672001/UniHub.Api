using UniHub.Domain.Entities.Identity;

namespace UniHub.Domain.Interface
{
    public interface IHaveUserIdEntityService
    {
        public Guid AspNetUserId { get; set; }

        public AspNetUser AspNetUser { get; set; }
    }
}