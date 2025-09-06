namespace UniHub.Domain.Interface
{
    public interface IHaveBaseSoftDeleteService
    {
        public bool IsDeleted { get; set; }
    }
}