namespace Receiver.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}