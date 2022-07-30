using Receiver.Models;

namespace Receiver.Repositories
{
    public interface IInboxRepository
    {
        Task<List<InboxMessage>> GetUnhandled();

        void Add(InboxMessage inbox);

        void Update(InboxMessage inbox);
    }
}