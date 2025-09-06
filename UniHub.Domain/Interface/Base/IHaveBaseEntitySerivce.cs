namespace UniHub.Domain.Interface
{
    public interface IHaveBaseEntitySerivce
    {
        public byte[] RowVersion { get; set; }
    }
}