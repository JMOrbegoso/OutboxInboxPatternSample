namespace OutboxProcessor.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}