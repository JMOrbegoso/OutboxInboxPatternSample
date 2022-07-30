using SenderApi.Models;

namespace SenderApi.Repositories
{
    public interface IOutboxRepository
    {
        void Add(OutboxMessage outbox);
    }
}
