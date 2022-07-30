using Microsoft.EntityFrameworkCore;
using OutboxProcessor.Models;

namespace OutboxProcessor.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly AppDbContext _appContext;

        public OutboxRepository(AppDbContext appContext)
        {
            this._appContext = appContext;
        }

        public async Task<List<OutboxMessage>> GetUnsentOutboxMessages()
        {
            return await this._appContext.OutboxMessages
                .Where(c => c.SentOn == null)
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();
        }

        public void Update(OutboxMessage outbox)
        {
            this._appContext.Update(outbox);
        }
    }
}