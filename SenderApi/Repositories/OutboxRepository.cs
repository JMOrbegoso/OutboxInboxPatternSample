using SenderApi.Models;

namespace SenderApi.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly AppDbContext _appContext;

        public OutboxRepository(AppDbContext appContext)
        {
            this._appContext = appContext;
        }

        public void Add(OutboxMessage outbox)
        {
            this._appContext.OutboxMessages.Add(outbox);
        }
    }
}