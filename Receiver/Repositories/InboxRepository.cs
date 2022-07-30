using Microsoft.EntityFrameworkCore;
using Receiver.Models;

namespace Receiver.Repositories
{
    public class InboxRepository : IInboxRepository
    {
        private readonly AppDbContext _appContext;

        public InboxRepository(AppDbContext appContext)
        {
            this._appContext = appContext;
        }

        public async Task<List<InboxMessage>> GetUnhandled()
        {
            return await this._appContext.InboxMessages
                .Where(c => c.HandledOn == null)
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();
        }

        public void Add(InboxMessage inbox)
        {
            this._appContext.InboxMessages.Add(inbox);
        }

        public void Update(InboxMessage inbox)
        {
            this._appContext.InboxMessages.Update(inbox);
        }
    }
}