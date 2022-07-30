using OutboxProcessor.Models;

namespace OutboxProcessor.Repositories
{
    public interface IOutboxRepository
    {
        Task<List<OutboxMessage>> GetUnsentOutboxMessages();
        void Update(OutboxMessage outbox);
    }
}