namespace SenderApi.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}