namespace UniHub.Infrastructure;

public interface IUnitOfWork
{
    int Commit();

    Task<int> CommitAsync();
}